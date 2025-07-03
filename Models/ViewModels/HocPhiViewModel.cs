namespace hehehe.Models.ViewModels
{
    public class HocPhiViewModel
    {
        public decimal BaoHiemYTe { get; set; } = 563220;
        public decimal KhamSucKhoe { get; set; } = 150000;
        public decimal HoSoNhapHoc { get; set; } = 350000;
        public decimal TheSinhVien { get; set; } = 50000;

        public decimal TongTien =>
            BaoHiemYTe + KhamSucKhoe + HoSoNhapHoc + TheSinhVien;
    }
}

