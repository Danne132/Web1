using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_64130005.Models;
using Project_64130005.Utils;

namespace Project_64130005.Areas.Admin_64130005.Controllers
{
    public class NhanVien_64130005Controller : Controller
    {
        private readonly Project_64130005Entities1 db = new Project_64130005Entities1();

        // GET: Admin_64130005/NhanVien_64130005
        public ActionResult Index(String searchString)
        {
            NhanVien nv = (NhanVien)Session["NV"];
            if(nv.MaLoaiNV == "BH" || nv.MaLoaiNV == "KH")
            {
                return RedirectToAction("Index", "Home");
            }
            var dsNhanVien = (from s in db.NhanViens select s).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                ViewBag.NhanVien = dsNhanVien.Where(s => s.HoTenNV.ToLower().Contains(searchString)).ToList();
            }
            else
            {
                ViewBag.NhanVien = dsNhanVien;
            }
            return View();
        }

        // GET: Admin_64130005/NhanVien_64130005/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // GET: Admin_64130005/NhanVien_64130005/Create
        public ActionResult Create()
        {
            ViewBag.MaLoaiNV = new SelectList(db.LoaiNVs, "MaLoaiNV", "TenLoaiNV");
            return View();
        }

        // POST: Admin_64130005/NhanVien_64130005/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file ,[Bind(Include = "MaNV,HoTenNV,Email,DiaChi,NgaySinh,GioiTinh,SDT,AnhNV,MatKhau,MaLoaiNV")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                nhanVien.MaNV = LayMaNV(nhanVien.MaLoaiNV);
                nhanVien.CreatedAt = DateTime.Now;
                string anh = "place_holder.jpg";
                if(file != null)
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    String path = System.IO.Path.Combine(Server.MapPath("~/Images/NhanVien"), pic);
                    file.SaveAs(path);
                    anh = pic;
                    using(MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                }
                nhanVien.AnhNV = anh;   
                db.NhanViens.Add(nhanVien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoaiNV = new SelectList(db.LoaiNVs, "MaLoaiNV", "TenLoaiNV", nhanVien.MaLoaiNV);
            return View(nhanVien);
        }

        // GET: Admin_64130005/NhanVien_64130005/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiNV = new SelectList(db.LoaiNVs, "MaLoaiNV", "TenLoaiNV", nhanVien.MaLoaiNV);
            return View(nhanVien);
        }

        // POST: Admin_64130005/NhanVien_64130005/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNV,HoTenNV,Email,DiaChi,NgaySinh,GioiTinh,SDT,AnhNV,MatKhau,MaLoaiNV,CreatedAt,UpDatedAt")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhanVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoaiNV = new SelectList(db.LoaiNVs, "MaLoaiNV", "TenLoaiNV", nhanVien.MaLoaiNV);
            return View(nhanVien);
        }

        // GET: Admin_64130005/NhanVien_64130005/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // POST: Admin_64130005/NhanVien_64130005/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NhanVien nhanVien = db.NhanViens.Find(id);
            db.NhanViens.Remove(nhanVien);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private string LayMaNV(string maCV)
        {
            // Đếm số sách thuộc loại này
            int soLuong = db.NhanViens.Count(s => s.MaLoaiNV == maCV);

            // Tạo mã sách mới
            string maSachMoi = $"{maCV}{(soLuong + 1).ToString("D3")}";
            return maSachMoi;
        }

    }
}
