using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_64130005.Models;
using System.IO;
using System.Diagnostics;

namespace Project_64130005.Controllers
{
    public class KhachHang_64130005Controller : Controller
    {
        private Project_64130005Entities1 db = new Project_64130005Entities1();

        // GET: KhachHang_64130005
        public async Task<ActionResult> Index()
        {
            return View();
        }

        public ActionResult Register_64130005()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register_64130005(KhachHang khachhang)
        {
            if (ModelState.IsValid)
            {
                var check = db.KhachHangs.FirstOrDefault(s => s.Emai == khachhang.Emai);
                if (check == null)
                {
                    string maKH = LayMaKH();
                    String anh = "user.jpg";
                    khachhang.AnhKH = anh;
                    khachhang.MaKH = maKH;
                    db.KhachHangs.Add(khachhang);
                    db.SaveChanges();
                    return RedirectToAction("Login_64130005");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }
            }
            return View();
        }

        public ActionResult Login_64130005()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login_64130005(string emai, string matKhau)
        {
            if (emai == null || matKhau == null) Debug.WriteLine("Thiếu");
            if (ModelState.IsValid)
            {
                Debug.WriteLine("Email: " + emai + " Pass: " + matKhau);
                KhachHang kh = db.KhachHangs.FirstOrDefault(s => s.Emai.Equals(emai) && s.MatKhau.Equals(matKhau));
                if (kh != null)
                {
                    Session["Taikhoan"] = kh;
                    Session["NV"] = null;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //ModelState.AddModelError("", "Sai email hoặc mật khẩu");
                    ViewBag.error = "Vui lòng thử lại, xin cảm ơn.";
                    return View();
                }
            }
            return View();
        }

        // GET: KhachHang_64130005/Details/5
        public async Task<ActionResult> Details()
        {
            KhachHang kh = (KhachHang)Session["Taikhoan"];
            KhachHang khachHang = db.KhachHangs.Find(kh.MaKH);

            if (kh != null && khachHang != null)
            {
                return View(khachHang);
            }
            return HttpNotFound();
        }


        // POST: KhachHang_64130005/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public ActionResult Edit(string idUser)
        {
            KhachHang khsession = (KhachHang)Session["Taikhoan"];
            KhachHang kh = db.KhachHangs.Find(idUser);
            kh.ConfirmPassword = kh.MatKhau;
            if (khsession.MatKhau != kh.MaKH)
            {
                return HttpNotFound();

            }
            return View(kh);
        }

        public ActionResult Edit([Bind(Include = "MaKH,HoKH,TenKH,Emai,MatKhau,AnhKH,DiaChi,NgaySinh,CCCD,SDT, MatKhau, ConfirmPassword")] KhachHang kh, HttpPostedFileBase file)
        {
            KhachHang khachhang = db.KhachHangs.Find(kh.MaKH);
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    String path = System.IO.Path.Combine(
                                            Server.MapPath("~/Images/KhachHang"), pic);
                    file.SaveAs(path);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                    khachhang.AnhKH = pic;
                }
                khachhang.TenKH = kh.TenKH;
                khachhang.HoKH = kh.HoKH;
                khachhang.Emai = kh.Emai;
                khachhang.DiaChi = kh.DiaChi;
                khachhang.ConfirmPassword = kh.ConfirmPassword;

                if (kh.NgaySinh != null)
                {
                    khachhang.NgaySinh = kh.NgaySinh;
                }
                if (kh.CCCD != null)
                {
                    khachhang.CCCD = kh.CCCD;
                }

                if (kh.SDT != null)
                {
                    khachhang.SDT = kh.SDT;
                }
                db.Entry(khachhang).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("ProFile");
            }
            return View(kh);
        }

        private string LayMaKH()
        {
            int soLuong = db.KhachHangs.Count();

            // Tạo mã sách mới
            string maSachMoi = $"{"KH"}{(soLuong + 1).ToString("D3")}";
            return maSachMoi;
        }


    }
}
