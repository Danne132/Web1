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
using System.Web.Services.Description;
using System.Web.UI;

namespace Project_64130005.Areas.Admin_64130005.Controllers
{
    public class DonHang_64130005Controller : Controller
    {
        private Project_64130005Entities1 db = new Project_64130005Entities1();

        // GET: Admin_64130005/DonHang_64130005
        public async Task<ActionResult> Index(string searchStr, string sort, int? page)
        {
            const int pageSize = 10;
            int pageNumber = page ?? 1;
            var orders = db.DonHangs.ToList();
            // Tìm kiếm đơn hàng = Sđt - Sprin 6
            if (!String.IsNullOrEmpty(searchStr))
            {
                searchStr = searchStr.ToLower();
                ViewBag.searchStr = searchStr;
                orders = orders.Where(p => p.KhachHang.SDT.ToString().ToLower().Contains(searchStr)).ToList();
                ViewBag.orderList = orders;
            }
            else
            {
                Sort(sort);
            }

            return View();
        }

        public void Sort(string sort)
        {
            ViewBag.Sort = sort;
            var orderList = (from s in db.DonHangs select s).ToList();
            foreach (var order in orderList)
            {
                if (order.NgayGiao < DateTime.Now && order.MaTrangThai == "Đang giao hàng")
                {
                    DonHang editOrder = db.DonHangs.Find(order.MaDH);
                    editOrder.MaTrangThai = "Hoàn thành";
                    editOrder.MaThanhToan = null;
                    db.Entry(editOrder).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            if (String.IsNullOrEmpty(sort))
            {
                ViewBag.orderList = orderList.OrderBy(s => s.NgayDat);
            }
            else
            {
                switch (sort)
                {
                    case "Wait":
                        ViewBag.orderList = orderList.Where(s => s.MaTrangThai == "Chưa giao hàng");
                        break;
                    case "Deli":
                        ViewBag.orderList = orderList.Where(s => s.MaTrangThai == "Đang giao hàng");
                        break;
                    case "Done":
                        ViewBag.orderList = orderList.Where(s => s.MaTrangThai == "Hoàn thành");
                        break;
                    case "Cancel":
                        ViewBag.orderList = orderList.Where(s => s.MaTrangThai == "Đã hủy");
                        break;
                    default:
                        ViewBag.orderList = orderList.Where(s => s.KhachHang.SDT.ToString().Contains(sort));
                        break;
                }
            }
        }

        // GET: Admin_64130005/DonHang_64130005/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = await db.DonHangs.FindAsync(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }


        // GET: Admin_64130005/DonHang_64130005/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = await db.DonHangs.FindAsync(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HoKH", donHang.MaKH);
            ViewBag.NVDuyetDon = new SelectList(db.NhanViens, "MaNV", "HoTenNV", donHang.NVDuyetDon);
            ViewBag.NVGiaoHang = new SelectList(db.NhanViens, "MaNV", "HoTenNV", donHang.NVGiaoHang);
            ViewBag.MaThanhToan = new SelectList(db.ThanhToans, "MaThanhToan", "TenThanhToan", donHang.MaThanhToan);
            ViewBag.MaTrangThai = new SelectList(db.TrangThaiDHs, "TrangThai", "TrangThai", donHang.MaTrangThai);
            return View(donHang);
        }

        // POST: Admin_64130005/DonHang_64130005/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaDH,MaKH,TenNguoiNhan,MaTrangThai,DiaChi,MaThanhToan,NgayDat,NgayGiao,TongTien,TongSoLuong,NVDuyetDon,NVGiaoHang")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donHang).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HoKH", donHang.MaKH);
            ViewBag.NVDuyetDon = new SelectList(db.NhanViens, "MaNV", "HoTenNV", donHang.NVDuyetDon);
            ViewBag.NVGiaoHang = new SelectList(db.NhanViens, "MaNV", "HoTenNV", donHang.NVGiaoHang);
            ViewBag.MaThanhToan = new SelectList(db.ThanhToans, "MaThanhToan", "TenThanhToan", donHang.MaThanhToan);
            ViewBag.MaTrangThai = new SelectList(db.TrangThaiDHs, "TrangThai", "TrangThai", donHang.MaTrangThai);
            return View(donHang);
        }

        // GET: Admin_64130005/DonHang_64130005/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = await db.DonHangs.FindAsync(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }

        // POST: Admin_64130005/DonHang_64130005/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            DonHang donHang = await db.DonHangs.FindAsync(id);
            db.DonHangs.Remove(donHang);
            await db.SaveChangesAsync();
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
    }
}
