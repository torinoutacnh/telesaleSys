using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Labixa.Areas.Admin.ViewModel;
using Outsourcing.Service;
using Outsourcing.Data.Models;
using Outsourcing.Core.Common;
using Outsourcing.Core.Extensions;
using WebGrease.Css.Extensions;
using Outsourcing.Core.Framework.Controllers;

namespace Labixa.Areas.Admin.Controllers
{

    public partial class ProductController : Controller
    {
        #region Field

        readonly IProductService _productService;
        readonly IProductCategoryService _productCategoryService;
        readonly IBrandService brandService;

        readonly IProductAttributeService _productAttributeService;
        readonly IProductAttributeMappingService _productAttributeMappingService;

        readonly IPictureService _pictureService;
        readonly IProductPictureMappingService _productPictureMappingService;


        readonly IProductRelationshipService _productRelationShipService;
        readonly ILevelService _levelService;
        readonly IDataUserService _dataUserService;

        #endregion

        #region Ctor
        public ProductController(IProductService productService, IProductCategoryService productCategoryService,
            IProductAttributeService productAttributeService, IProductAttributeMappingService productAttributeMappingService,
            IPictureService pictureService, IProductPictureMappingService productPictureMappingService,
            IProductRelationshipService productRelationShipService, ILevelService levelShipService, IDataUserService dataUserService,
            IBrandService _brandService)
        {
            this.brandService = _brandService;
            this._productService = productService;
            this._productCategoryService = productCategoryService;
            this._productAttributeService = productAttributeService;
            this._productAttributeMappingService = productAttributeMappingService;
            this._pictureService = pictureService;
            this._productPictureMappingService = productPictureMappingService;
            this._productRelationShipService = productRelationShipService;
            this._levelService = levelShipService;
            this._dataUserService = dataUserService;
        }
        #endregion
        //void adddata(Product item)
        //{

        //    ProductAttributeMapping obj = new ProductAttributeMapping();
        //    obj.IsRequired = false;
        //    obj.ProductAttributeId = 13;
        //    obj.ProductId = item.Id;
        //    obj.Value = "true";
        //    _productAttributeMappingService.CreateProductAttributeMapping(obj);
        //}
        int _BRAND_ID = 0;
        string _BRAND_CODE = "THT";
        string _BRAND_NAME = "Toàn Hệ Thống";
        public void GetCookie()
        {
            HttpCookie brandIdCookie = Request.Cookies["BrandId"];
            HttpCookie brandNameCookie = Request.Cookies["BrandCode"];
            HttpCookie brandCodeCookie = Request.Cookies["BrandName"];
            if (brandIdCookie != null)
            {
                _BRAND_ID = int.Parse(brandIdCookie.Value);
                _BRAND_CODE = brandNameCookie.Value;
                _BRAND_NAME = brandCodeCookie.Value;
            }
        }
        public ActionResult Index()
        {
            var products = new List<Product>();
            GetCookie();
            if (_BRAND_ID==0)
            {
            products = _productService.GetProducts().ToList();
            }
            else
            {
                products = _productService.GetProducts().Where(p=>p.BrandId == _BRAND_ID).ToList();
            }
            //foreach (var item in products)
            //{
            //    adddata(item);
            //}
            //foreach (var item in products)
            //{
            //    ProductAttributeMapping obj = new ProductAttributeMapping();
            //    obj.IsRequired = false;
            //    obj.Value = "true";
            //    obj.DisplayOrder = 0;
            //    obj.ProductId = item.Id;
            //    obj.ProductAttributeId = 13;
            //    _productAttributeMappingService.CreateProductAttributeMapping(obj);
            ////adddata(item.Id);
            //}
            return View(model: products);
        }
        public ActionResult ReportLevel(int Id)
        {
            var model = _levelService.GetLevels().Where(p=>p.Position==Id).ToList();

            return View(model);
        }
        public ActionResult DataLevel(Level col, Level row)
        {
            var listCol = _dataUserService.GetDataUsers().Where(p => p.LevelId == col.Id).ToList();
            var listRow = _dataUserService.GetDataUsers().Where(p => p.LevelId == row.Id).ToList();
            float persent = (listCol.Count / (listRow.Count == 0 ? 1 : listRow.Count)) * 100;
            var model = int.Parse(persent.ToString());
            return PartialView("_DataLevel", model);
        }
        public ActionResult Create()
        {
            GetCookie();
            ViewBag.BrandList = brandService.GetBrands().ToSelectListItems(-1);
            //Get the list category
            var listProductCategory = _productCategoryService.GetProductCategories().Where(p => p.BrandId == _BRAND_ID).ToSelectListItems(-1);
            var product = new ProductFormModel { ListProductCategory = listProductCategory };
            return View(product);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [ValidateInput(false)]
        public ActionResult Create(ProductFormModel newProduct, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                //Mapping to domain
                Product product = Mapper.Map<ProductFormModel, Product>(newProduct);
                if (String.IsNullOrEmpty(product.Slug))
                {
                    product.Slug = StringConvert.ConvertShortName(product.Name);
                }

                //Create Product
                _productService.CreateProduct(product);

                //Add ProductAttribute after product created
                //product.ProductAttributeMappings = new Collection<ProductAttributeMapping>();
                //var listAttributeId = _productAttributeService.GetProductAttributes().Select(p => p.Id);
                //foreach (var id in listAttributeId)
                //{
                //    product.ProductAttributeMappings.Add(
                //        new ProductAttributeMapping() { ProductAttributeId = id, ProductId = product.Id });

                //}
                //Add Picture default for Labixa
                product.ProductPictureMappings = new Collection<ProductPictureMapping>();
                for (int i = 0; i < 4; i++)
                {
                    var newPic = new Picture();
                    bool ismain = i == 0;
                    _pictureService.CreatePicture(newPic);
                    product.ProductPictureMappings.Add(
                        new ProductPictureMapping()
                        {
                            PictureId = newPic.Id,
                            ProductId = product.Id,
                            IsMainPicture = ismain,
                            DisplayOrder = 0,
                        });
                }
                _productService.EditProduct(product);


                //create product relation
                #region
                ProductRelationship item2 = new ProductRelationship();               
                for (int i = 0; i < 3; i++)
                {
                    ProductRelationship item = new ProductRelationship();
                    item.ProductId = product.Id;
                    item.ProductRelateId = product.Id;
                    item.isAvailable = true;
                    _productRelationShipService.CreateProductRelationship(item);
                }
                #endregion
                //Save all after edit
                return continueEditing ? RedirectToAction("Edit", "Product", new { productId = product.Id })
                                : RedirectToAction("Index", "Product");
            }
            else
            {
                ViewBag.BrandList = brandService.GetBrands().ToSelectListItems(-1);
                var listProductCategory = _productCategoryService.GetProductCategories().ToSelectListItems(-1);
                newProduct.ListProductCategory = listProductCategory;
                return View("Create", newProduct);
            }
        }

        [HttpGet]
        public ActionResult Edit(int productId)
        {
            GetCookie();
            var product = _productService.GetProductById(productId);
            if (product.BrandId != null)
            {
            ViewBag.BrandList = brandService.GetBrands().ToSelectListItems(product.BrandId.Value);
            }
            else
            {
                ViewBag.BrandList = brandService.GetBrands().ToSelectListItems(-1);
            }
            ProductFormModel productFormModel = Mapper.Map<Product, ProductFormModel>(product);
            productFormModel.ListProductCategory = _productCategoryService.GetProductCategories().Where(p => p.BrandId == _BRAND_ID).ToSelectListItems(-1);
            productFormModel.ListProducts = _productService.GetAllProducts().ToSelectListItems(-1);
            var list = _productRelationShipService.GetProductById(productId);
            //productFormModel.Id1 = list.ToList()[0].ProductRelateId;
            //productFormModel.Id2 = list.ToList()[1].ProductRelateId;
            //productFormModel.Id3 = list.ToList()[2].ProductRelateId;
            return View(model: productFormModel);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [ValidateInput(false)]
        public ActionResult Edit(ProductFormModel productToEdit, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                //Mapping to domain
                Product product = Mapper.Map<ProductFormModel, Product>(productToEdit);
                
                    product.Slug = StringConvert.ConvertShortName(product.Name) + "-"+product.Id;
                //this funcion not update all relationship value.
                _productService.EditProduct(product);
                //update attribute
                //foreach (var mapping in product.ProductAttributeMappings)
                //{
                //    _productAttributeMappingService.EditProductAttributeMapping(mapping);
                //}
                //update productpicture URL
                foreach (var picture in product.ProductPictureMappings)
                {
                    _productPictureMappingService.EditProductPictureMapping(picture);
                    _pictureService.EditPicture(picture.Picture);
                }
                //add tour relation
                #region [add tour relation]
                //var listProductRelation = _productRelationShipService.GetProductById(product.Id);
                //var item1 = listProductRelation.ToList()[0];
                //var item2 = listProductRelation.ToList()[1];
                //var item3 = listProductRelation.ToList()[2];
                //item1.ProductRelateId = productToEdit.Id1;
                //item2.ProductRelateId = productToEdit.Id2;
                //item3.ProductRelateId = productToEdit.Id3;
                //_productRelationShipService.EditProductRelationship(item1);
                //_productRelationShipService.EditProductRelationship(item2);
                //_productRelationShipService.EditProductRelationship(item3);
                #endregion
                return continueEditing ? RedirectToAction("Edit", "Product", new { productId = product.Id })
                      : RedirectToAction("Index", "Product");
            }
            else
            {
                if (productToEdit.BrandId != null)
                {
                    ViewBag.BrandList = brandService.GetBrands().ToSelectListItems(productToEdit.BrandId.Value);
                }
                else
                {
                    ViewBag.BrandList = brandService.GetBrands().ToSelectListItems(-1);
                }
                var listProductCategory = _productCategoryService.GetProductCategories().ToSelectListItems(-1);
                productToEdit.ListProductCategory = listProductCategory;
                return View("Edit", productToEdit);
            }
        }


        public ActionResult Delete(int productId)
        {
            _productService.DeleteProduct(productId);
            return RedirectToAction("Index");
        }
    }
}