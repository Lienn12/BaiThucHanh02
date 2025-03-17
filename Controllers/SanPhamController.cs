using BaiThucHanh02.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BaiThucHanh02.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        private SanPhamDBEntities4 db = new SanPhamDBEntities4();

        public ActionResult Index()
        {
            var sanPhams = db.SanPhams.ToList();
            return View(sanPhams);
        }

        public ActionResult AddForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(SanPham sanPham, HttpPostedFileBase ImageFile)
        {
            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                string folderPath = Server.MapPath("~/Content/Images");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string fileName = Path.GetFileName(ImageFile.FileName);
                string filePath = Path.Combine(folderPath, fileName);
                ImageFile.SaveAs(filePath);

                sanPham.Image = "/Content/Images/" + fileName;
            }
            db.SanPhams.Add(sanPham);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult EditForm(int id)
        {
            var sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }
        [HttpPost]
        public ActionResult Edit(SanPham sanPham, HttpPostedFileBase ImageFile)
        {
            var existingSanPham = db.SanPhams.Find(sanPham.Id);
            if (existingSanPham == null)
            {
                return HttpNotFound();
            }

            existingSanPham.Name = sanPham.Name;
            existingSanPham.Price = sanPham.Price;
            existingSanPham.Description = sanPham.Description;
            existingSanPham.Discount = sanPham.Discount;

            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                string folderPath = Server.MapPath("~/Content/Images");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string fileName = Path.GetFileName(ImageFile.FileName);
                string filePath = Path.Combine(folderPath, fileName);
                ImageFile.SaveAs(filePath);

                existingSanPham.Image = "/Content/Images/" + fileName; 
            }

            db.SaveChanges();
            return RedirectToAction("Index"); 
        }
        public ActionResult Details(int id)
        {
            var sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }
        public ActionResult Delete(int id)
        {
            var sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }

            // Xóa ảnh khỏi thư mục nếu tồn tại
            if (!string.IsNullOrEmpty(sanPham.Image))
            {
                string filePath = Server.MapPath(sanPham.Image);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            db.SanPhams.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}