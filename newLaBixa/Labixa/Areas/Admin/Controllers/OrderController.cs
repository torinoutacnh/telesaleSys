using Outsourcing.Core.Extensions;
using Outsourcing.Data.Models;
using Outsourcing.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Labixa.Areas.Admin.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        #region Field
        readonly IOrderItemService orderItemService;
        readonly ILogdiaryService _LogdiaryService;
        readonly IUserDataMappingService userDataMappingService;
        readonly IProductService productService;
        readonly ILevelService levelService;
        readonly IStoreService storeService;
        readonly IDataUserService dataUserService;
        #endregion
        #region ctor
        public OrderController(IOrderItemService orderItemService, ILogdiaryService _LogdiaryService, IUserDataMappingService userDataMappingService
            , IProductService productService, IDataUserService dataUserService, ILevelService levelService, IStoreService _storeService)
        {
            this.orderItemService = orderItemService;
            this._LogdiaryService = _LogdiaryService;
            this.userDataMappingService = userDataMappingService;
            this.productService = productService;
            this.dataUserService = dataUserService;
            this.levelService = levelService;
            storeService = _storeService;
        }
        #endregion


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
            return View();
        }
        public ActionResult Create(int dataMappingId)
        {
            GetCookie();
            var mapping = userDataMappingService.GetUserDataMappingById(dataMappingId);
            var customer = dataUserService.GetDataUserById(mapping.DataUserId);
            var date = DateTime.Now.ToLongDateString();
            var logDiary = new Logdiary()
            {
                CodeLogDiary = "",
                Content = "Mua hàng ngày " + date,
                DateCreated = DateTime.Now,
                IsActive = false,
                Deleted = true,
                UserDataId = mapping.DataUserId,
                LastEditedTime = DateTime.Now,
                StatusCall = "BUY PRODUCT",
                LinkFile = "",
                TotalCall_Second = 0,
                LevelId = customer.LevelId,
                NameLevel = customer.Level.CodeLevel + "-" + customer.Level.LevelName,
                temp1 = mapping.UserId,
                TeleName = mapping.User.FirstName + " " + mapping.User.LastName,
                TeleId = mapping.UserId,
                //User.Identity.GetUserId(),
                TotalCallAgain = 0,
                TotalCallSucceed = 0,
                TotalNotCall = 0,
                TotalPayment = 0,
                TotalStopCall = 0,
                Price_1 = 0,
                Position = 0,
            };
            var currentLogDiary = _LogdiaryService.CreateLogdiaryWithResponse(logDiary);
            //var currentLogDiary = _LogdiaryService.GetLogdiaryById(336);
            // var currentLogDiary = _LogdiaryService.GetLogdiaryById(1);
            var listProduct = productService.GetAllProducts().Where(p => p.BrandId == _BRAND_ID).ToList();
            ViewBag.ListProduct = listProduct;

            var listStore = storeService.GetStores().Where(p => p.BrandId == _BRAND_ID).ToSelectListItemsStore(currentLogDiary.Store_Code);
            ViewBag.listStore = listStore;

            ViewBag.CustomerName = customer.FirstName + " " + customer.LastName;
            ViewBag.CustomerPhone = customer.PhoneNumber;
            ViewBag.CustomerAddress = customer.Address;
            return View(model: currentLogDiary);
        }
        public ActionResult UpdateStore(int LogId, string storeCode)
        {
            var log = _LogdiaryService.GetLogdiaryById(LogId);
            log.Store_Code = storeCode;
            _LogdiaryService.EditLogdiary(log);
            var listStore = storeService.GetStores().Where(p => p.BrandId == _BRAND_ID).ToSelectListItemsStore(log.Store_Code);
            ViewBag.listStore = listStore;
            return PartialView("_storeUpdate");
        }
        public ActionResult ListOrderItems(int logId)
        {
            var orderItems = orderItemService.GetOrderItemsByLogId(logId);
            ViewBag.LogId = logId;
            var totalPrice = 0;
            foreach (var item in orderItems)
            {
                totalPrice += item.Price;
            }
            ViewBag.TotalPrice = totalPrice;
            var logDiary = _LogdiaryService.GetLogdiaryById(logId);
            logDiary.Price_1 = totalPrice;
            _LogdiaryService.EditLogdiary(logDiary);
            return PartialView("_ListOrderItems", orderItems);
        }
        public ActionResult Finish(int total,int logId,string CodeOrderTemp2="",string DiemDenTemp3="", string DiemDiTemp4="", string dateLastEdit="",string Content="")
        {
            var entity = _LogdiaryService.GetLogdiaryById(logId);
            if (entity != null)
            {
                var totalPrice = 0;
                var orderItems = orderItemService.GetOrderItemsByLogId(logId);
                foreach (var item in orderItems)
                {
                    totalPrice += item.Price;
                }
                entity.Deleted = false;
                entity.IsActive = true;
                entity.temp2 = CodeOrderTemp2;
                entity.temp3 = DiemDenTemp3;
                entity.Content = entity.Content;
                entity.temp4 = DiemDiTemp4;
                entity.LastEditedTime = DateTime.Parse(dateLastEdit);
                if (total == 0)
                {
                    entity.Price_1 = totalPrice;
                }
                else
                {
                    entity.Price_1 = double.Parse(total.ToString());
                }
                _LogdiaryService.EditLogdiary(entity);
            }
            return RedirectToAction("Close", "autocall");
        }
        [HttpPost]
        public ActionResult AddItems(int productId, int logId)
        {
            var product = productService.GetProductById(productId);
            var entity = orderItemService.GetOrderItems().Where(i => i.ProductId == productId && i.LogdiaryId == logId).FirstOrDefault();
            if (entity == null)
            {
                var orderI = new OrderItem()
                {
                    LogdiaryId = logId,
                    ProductId = productId,
                    Price = product.Price,
                    Quantity = 1,
                    StoreId = 1,
                    StoreName = 1,
                };
                orderItemService.CreateOrderItem(orderI);
            }
            else
            {
                entity.Quantity += 1;
                entity.Price += product.Price;
                orderItemService.EditOrderItem(entity);
            }
            var lId = logId;
            //var orderItems = orderItemService.GetOrderItemsByLogId(logId);
            //return PartialView("_ListOrderItems", orderItems);
            return RedirectToAction("ListOrderItems", "order", new { logId = lId });
        }
        [HttpPost]
        public ActionResult EditQuantity(int quantity, int itemId, int logId)
        {
            var entity = orderItemService.GetOrderItemById(itemId);
            var lId = logId;
            if (entity != null)
            {
                if (entity.Quantity != quantity && quantity > 0)
                {
                    var price = entity.Product.Price;
                    entity.Quantity = quantity;
                    entity.Price = price * quantity;
                    orderItemService.EditOrderItem(entity);
                }
            }
            var orderItems = orderItemService.GetOrderItemsByLogId(logId);
            //return PartialView("_ListOrderItems", orderItems);
            return RedirectToAction("ListOrderItems", "order", new { logId = lId });
        }
        [HttpPost]
        public ActionResult DeleteItem(int itemId, int logId)
        {
            orderItemService.DeleteOrderItem(itemId);
            //var orderItems = orderItemService.GetOrderItemsByLogId(logId);
            //return PartialView("_ListOrderItems", orderItems);
            var lId = logId;
            return RedirectToAction("ListOrderItems", "order", new { logId = lId });
        }
        [HttpPost]
        public ActionResult DeleteOrder(int logid)
        {
            _LogdiaryService.DeleteLogdiary(logid);
            return null;
        }
    }
}