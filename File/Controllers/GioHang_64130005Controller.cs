using Project_64130005.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_64130005.Controllers
{
    public class GioHang_64130005Controller : Controller
    {
        private readonly Project_64130005Entities1 db = new Project_64130005Entities1();
        // GET: GioHang_64130005
        public ActionResult Index()
        {
            List<GioHang_64130005> listGioHang = Laygiohang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(listGioHang);
        }
        public List<GioHang_64130005> Laygiohang()
        {
            List<GioHang_64130005> listGiohang = Session["Giohang"] as List<GioHang_64130005>;

            if (listGiohang == null)
            {
                listGiohang = new List<GioHang_64130005>();
                Session["Giohang"] = listGiohang;

            }
            return listGiohang;
        }
        public int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang_64130005> listGiohang = Session["Giohang"] as List<GioHang_64130005>;
            if (listGiohang != null)
            {
                iTongSoLuong = listGiohang.Sum(n => n.SoLuong);
            }
            return iTongSoLuong;
        }
        private int TongTien()
        {
            int iTongTien = 0;
            List<GioHang_64130005> listGiohang = Session["Giohang"] as List<GioHang_64130005>;
            if (listGiohang != null)
            {
                iTongTien = listGiohang.Sum(n => n.ThanhTien);
            }
            return iTongTien;
        }
        public ActionResult Xoagiohang(string MaSach)
        {
            // Lấy giỏ hàng từ session
            List<GioHang_64130005> listGiohang = Laygiohang();
            // Kiểm tra sản phẩm có trong giỏ hàng hay không
            GioHang_64130005 product = listGiohang.SingleOrDefault(n => n.MaSach == MaSach);
            //nếu tồn tại thì sửa số lượng
            if (product != null)
            {
                listGiohang.RemoveAll(n => n.MaSach == MaSach);
                return RedirectToAction("Index");
            }
            if (listGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index");
        }
        public ActionResult Capnhatgiohang(string MaSach, FormCollection f)
        {
            // Lấy giỏ hàng từ session
            List<GioHang_64130005> listGiohang = Laygiohang();
            // Kiểm tra sản phẩm có trong giỏ hàng hay không
            GioHang_64130005 product = listGiohang.SingleOrDefault(n => n.MaSach == MaSach);
            if (product != null)
            {
                product.SoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("Index");
        }
        public ActionResult DeleteAll()
        {
            List<GioHang_64130005> listGiohang = Laygiohang();
            listGiohang.Clear();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Themgiohang(string MaSach, string strURL)
        {
            // Lấy ra session giỏ hàng
            List<GioHang_64130005> listGiohang = Laygiohang();
            // Kiểm tra sản phẩm này có trong giỏ hàng chưa
            GioHang_64130005 product = listGiohang.Find(n => n.MaSach == MaSach);
            if (product == null)
            {
                product = new GioHang_64130005(MaSach);
                listGiohang.Add(product);
                return Redirect(strURL);
            }
            else
            {
                product.SoLuong++;
                return Redirect(strURL);
            }
        }
        [HttpGet]
        public ActionResult DatHang_64130005()
        {
            // Kiểm tra đăng nhập
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DatHangKhongTK_64130005");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Lấy giỏ hàng từ session 
            List<GioHang_64130005> listGiohang = Laygiohang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(listGiohang);
        }
        public ActionResult DatHang_64130005(FormCollection collection)
        {
            var dsProduct = (from s in db.Saches select s).ToList();
            //Thêm đơn hàng
            DonHang order = new DonHang();
            KhachHang khsession = (KhachHang)Session["Taikhoan"];
            KhachHang kh = db.KhachHangs.Find(khsession.MaKH);
            List<GioHang_64130005> gh = Laygiohang();
            order.MaDH = LayMaDH();
            order.MaKH = kh.MaKH;
            order.NgayDat = DateTime.Now;
            order.NgayGiao = order.NgayDat.Value.AddDays(3);
            order.MaTrangThai = "Chưa giao hàng";
            order.MaThanhToan = "TM";
            string DiaChi = collection["DiaChi"];
            string Tinh = collection["Tinh"];
            string Quan = collection["Quan"];
            string Phuong = collection["Phuong"];
            order.TenNguoiNhan = collection["TenNguoiNhan"];
            order.DiaChi = String.Concat(DiaChi, " ", Tinh, " ", Quan, " ", Phuong);
            order.TongTien = TongTien();
            order.TongSoLuong = TongSoLuong();
            order.NVDuyetDon = null;
            order.NVGiaoHang = null;
            db.DonHangs.Add(order);
            db.SaveChanges();
            foreach (var item in gh)
            {
                ChiTietDH ctdh = new ChiTietDH();
                ctdh.MaDH = order.MaDH;
                ctdh.MaSach = item.MaSach;
                ctdh.SoLuong = item.SoLuong;
                ctdh.ThanhTien = item.ThanhTien;
                db.ChiTietDHs.Add(ctdh);
            }
            db.SaveChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang_64130005", "GioHang_64130005");
        }
        public ActionResult DatHangKhongTK_64130005()
        {
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Lấy giỏ hàng từ session 
            List<GioHang_64130005> listGiohang = Laygiohang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(listGiohang);
        }
        public ActionResult DatHangKhongTK_64130005(FormCollection collection)
        {
            var dsProduct = (from s in db.Saches select s).ToList();
            //Thêm đơn hàng
            var HoTen = collection["HoTen"];
            var Sdt = collection["Sdt"];
            var Email = collection["Email"];

            KhachHang kh = new KhachHang();
            kh.HoKH = HoTen.ToString();
            kh.Emai = Email.ToString();
            String anh = "user.jpg";
            kh.AnhKH = anh;
            if (Sdt.ToString().Length == 10)
            {
                kh.SDT = Sdt.ToString();
            }
            else
            {
                kh.SDT = null;
            }
            db.KhachHangs.Add(kh);
            db.SaveChanges();
            DonHang order = new DonHang();
            List<GioHang_64130005> gh = Laygiohang();
            order.MaDH = LayMaDH();
            order.MaKH = kh.MaKH;
            order.NgayDat = DateTime.Now;
            order.NgayGiao = order.NgayDat.Value.AddDays(3);
            order.MaTrangThai = "Chưa giao hàng";
            order.MaThanhToan = null;
            var DiaChi = collection["DiaChi"];
            order.DiaChi = DiaChi.ToString();
            order.TongTien = TongTien();
            order.TongSoLuong = TongSoLuong();
            db.DonHangs.Add(order);
            db.SaveChanges();
            foreach (var item in gh)
            {
                ChiTietDH ctdh = new ChiTietDH();
                ctdh.MaDH = order.MaDH;
                ctdh.MaSach = item.MaSach;
                ctdh.SoLuong = item.SoLuong;
                ctdh.ThanhTien = item.ThanhTien;
                db.ChiTietDHs.Add(ctdh);
            }
            db.SaveChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang_64130005", "Giohang_64130005");
        }
        public ActionResult BuyNow(string MaSach)
        {
            List<GioHang_64130005> listGiohang = Laygiohang();
            // Kiểm tra sản phẩm này có trong giỏ hàng chưa
            GioHang_64130005 product = listGiohang.Find(n => n.MaSach == MaSach);
            if (product == null)
            {
                product = new GioHang_64130005(MaSach);
                listGiohang.Add(product);
            }
            else
            {
                product.SoLuong++;
            }
            return RedirectToAction("Index");
        }
        public ActionResult Xacnhandonhang_64130005()
        {
            return View();
        }
        /*public ActionResult Payment() // tạo yêu cầu thanh toán và chuyển hướng người dùng đến trang thanh toán
        {

            string url = ConfigurationManager.AppSettings["Url"];
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            string hashSecret = ConfigurationManager.AppSettings["HashSecret"];
            DateTime exTime = DateTime.Now.AddDays(1);
            PayLib pay = new PayLib();

            pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", (TongTien() * 100).ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", exTime.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", _db.Orders.ToList().LastOrDefault().ID.ToString() + 1); //mã hóa đơn
            //pay.AddRequestData("vnp_payment", order.Payment.ToString()); //mã hóa đơn


            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

            return Redirect(paymentUrl);
        }*/
        private string LayMaDH()
        {
            DateTime ngayHienTai = DateTime.Now;
            string ngay = ngayHienTai.ToString("ddMMyy");
            int soThuTu = db.DonHangs.Count(d => d.NgayDat == ngayHienTai.Date)+1;
            string soThuTuDinhDang = soThuTu.ToString("D4");
            return $"DH{ngay}{soThuTuDinhDang}";
        }
    }
}