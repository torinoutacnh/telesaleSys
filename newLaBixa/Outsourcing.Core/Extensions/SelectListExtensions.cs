using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Outsourcing.Core;
using System.ComponentModel;
using Outsourcing.Data.Models;


namespace Outsourcing.Core.Extensions
{
    public static class SelectListExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItems(
            this IEnumerable<User> user, string selectedId)
        {
            return

                user.OrderBy(f => f.Id)
                      .Select(f =>
                          new SelectListItem
                          {
                              Selected = (f.Id == selectedId),
                              Text = f.UserName,
                              Value = f.Id.ToString()
                          });
        }
        public static IEnumerable<SelectListItem> ToSelectListItems(
              this IEnumerable<SourceData> data, int selectedId)
        {
            return

                data.OrderBy(user => user.Id)
                      .Select(user =>
                          new SelectListItem
                          {
                              Selected = (user.Id == selectedId),
                              Text = user.Name,
                              Value = user.Id.ToString()
                          });
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(
              this IEnumerable<Brand> data, int selectedId)
        {
            return

                data.OrderByDescending(user => user.Id)
                      .Select(user =>
                          new SelectListItem
                          {
                              Selected = (user.Id == selectedId),
                              Text = user.Name + " - " + user.Code,
                              Value = user.Id.ToString()
                          });
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(
              this IEnumerable<DataUser> data, int selectedId)
        {
            return

                data.OrderBy(user => user.Id)
                      .Select(user =>
                          new SelectListItem
                          {
                              Selected = (user.Id == selectedId),
                              Text = user.FirstName + " " + user.LastName,
                              Value = user.Id.ToString()
                          });
        }


        
        


        public static IEnumerable<SelectListItem> ToSelectListItems(
      this IEnumerable<ChanelData> Chanel, int selectedId)
        {
            return Chanel.OrderBy(f => f.Id)
                      .Select(f =>
                          new SelectListItem
                          {
                              Selected = (f.Id == selectedId),
                              Text = f.Name,
                              Value = f.Id.ToString()
                          });
        }
        public static IEnumerable<SelectListItem> ToSelectListItems(
      this IEnumerable<Level> Level, int selectedId)
        {
            return Level.OrderBy(f => f.Id)
                      .Select(f =>
                          new SelectListItem
                          {
                              Selected = (f.Id == selectedId),
                              Text = f.CodeLevel + " - " + f.LevelName,
                              Value = f.Id.ToString()
                          });
        }
        public static IEnumerable<SelectListItem> ToSelectListItems(
             this IEnumerable<Product> product, int selectedId)
        {
            return

                product.OrderBy(f => f.Id)
                      .Select(f =>
                          new SelectListItem
                          {
                              Selected = (f.Id == selectedId),
                              Text = f.Name,
                              Value = f.Id.ToString()
                          });
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(
              this IEnumerable<ProductCategory> productCategories, int selectedId)
        {
            return

                productCategories.OrderBy(f => f.Id)
                      .Select(f =>
                          new SelectListItem
                          {
                              Selected = (f.Id == selectedId),
                              Text = f.Name,
                              Value = f.Id.ToString()
                          });
        }
        public static IEnumerable<SelectListItem> ToSelectListItems(
             this IEnumerable<Ward> ward, int selectedId)
        {
            return

                ward.OrderBy(f => f.Id)
                      .Select(f =>
                          new SelectListItem
                          {
                              Selected = (f.Id == selectedId),
                              Text = f.Name,
                              Value = f.Id.ToString()
                          });
        }
        public static IEnumerable<SelectListItem> ToSelectListItems(
             this IEnumerable<District> District, int selectedId)
        {
            return

                District.OrderBy(f => f.Id)
                      .Select(f =>
                          new SelectListItem
                          {
                              Selected = (f.Id == selectedId),
                              Text = f.Name,
                              Value = f.Id.ToString()
                          });
        }

        //public static IEnumerable<SelectListItem> ToSelectListItems(
        //   this IEnumerable<Brand> Brand, int selectedId)
        //{
        //    return

        //        Brand.OrderBy(f => f.Id)
        //              .Select(f =>
        //                  new SelectListItem
        //                  {
        //                      Selected = (f.Id == selectedId),
        //                      Text = f.Name,
        //                      Value = f.Id.ToString()
        //                  });
        //}
        public static IEnumerable<SelectListItem> ToSelectListItems(
          this IEnumerable<Store> Store, int selectedId)
        {
            return

                Store.OrderBy(f => f.Id)
                      .Select(f =>
                          new SelectListItem
                          {
                              Selected = (f.Id == selectedId),
                              Text = f.Name,
                              Value = f.Id.ToString()
                          });
        }
        public static IEnumerable<SelectListItem> ToSelectListItemsStore(
          this IEnumerable<Store> Store, string Code)
        {
            return

                Store.OrderBy(f => f.Id)
                      .Select(f =>
                          new SelectListItem
                          {
                              Selected = (f.Code.Equals(Code)),
                              Text = f.Name,
                              Value = f.Code.ToString()
                          });
        }
    }
}

