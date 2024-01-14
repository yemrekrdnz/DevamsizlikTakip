using System;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace OgrenciDevamsizlikTakipSistemi
{
    class Program
    {
        static void Main(string[] args)
        {
            // Doğru kullanıcı adı ve şifre
            string dogruKullaniciAdi = "arzukilitcicalayir";
            string dogruSifre = "12345";

            Console.WriteLine("** GİRİŞ EKRANI **");

            // kullanıcı adını iste
            Console.Write("Kullanıcı Adı: ");
            string kullaniciAdi = Console.ReadLine();

            // şifreyi iste ve şifreyi sansürleyecek metodu çağır
            Console.Write("Şifre: ");
            string sifre = sifreyiSansurle();

            // Kullanıcı adı ve şifre kontrolü
            if (KullaniciGirisDogrula(kullaniciAdi, sifre, dogruKullaniciAdi, dogruSifre)) // KullaniciGirisDogrula true ya da false dönecek
            {
                Console.WriteLine("\nGiriş Bilgileri Doğru! \nHoş geldiniz.");

                // programdaki metotları kullanmak için devamsızlık işlemlerini başlat
                DevamsizlikIslemleri devamsizlikIslemleri = new DevamsizlikIslemleri();
                int secim = 0;

                while (secim != 6)
                {
                    Console.WriteLine("\n----------------------------");
                    Console.WriteLine("1-Öğrenci Ekle");
                    Console.WriteLine("2-Öğrencileri Listele");
                    Console.WriteLine("3-Devamsızlık Girişi Yap");
                    Console.WriteLine("4-Öğrenci Sil");
                    Console.WriteLine("5-Öğrenci Bilgisini Güncelle");
                    Console.WriteLine("6-Çık");
                    Console.WriteLine("----------------------------");
                    Console.Write("Seçim Yap:");

                    string kullaniciSecimi = Console.ReadLine();

                    // kullanıcının girişinin sayı olup olmadığını kontrol et.
                    if (int.TryParse(kullaniciSecimi, out secim))
                    {

                    }

                    switch (secim)
                    {
                        case 1:
                            Console.Write("Öğrenci Adı:");
                            string ad = Console.ReadLine();

                            Console.Write("Soyadı:");
                            string soyad = Console.ReadLine();

                            Console.Write("Okul Numarası:");
                            string numara = Console.ReadLine();

                            if (int.TryParse(numara, out int no))
                            {
                                Ogrenci ogrenci = new Ogrenci(ad, soyad, no);
                                devamsizlikIslemleri.OgrenciEkle(ogrenci);
                            }
                            else
                            {
                                Console.WriteLine("\nÖğrenci Numarası Sayı Olarak Girilmelidir.");
                            }

                            break;

                        case 2:
                            devamsizlikIslemleri.OgrencileriListele();
                            break;

                        case 3:
                            Console.Write("Öğrencinin okul numarası: ");
                            string okulnumarasi = Console.ReadLine();

                            // kullanıcının girdiği değerin sayı olup olmadığını kontrol et
                            if (int.TryParse(okulnumarasi, out int okulno))
                            {
                                Console.Write("Devamsızlık Sayısı: ");
                                int devamsizliksayisi = Convert.ToInt32(Console.ReadLine());

                                devamsizlikIslemleri.DevamsizlikGirisiYap(okulno, devamsizliksayisi);
                            }
                            else
                            {
                                Console.WriteLine("\nÖğrenci Numarası Sayı Olarak Girilmelidir.");
                            }

                            break;

                        case 4:
                            Console.Write("Silinecek Öğrencinin Okul Numarası: ");
                            int ogrenciNumarasi = Convert.ToInt32(Console.ReadLine());
                            devamsizlikIslemleri.OgrenciyiSil(ogrenciNumarasi);

                            break;

                        case 5:
                            Console.Write("Bilgileri Güncellenecek Öğrencinin okul Numarası: ");
                            string ogrenciOkulNo = Console.ReadLine();

                            // kullanıcının girdiği değerin sayı olup olmadığını kontrol et
                            if (int.TryParse(ogrenciOkulNo, out int okulNumarasi))
                            {
                                devamsizlikIslemleri.OgrenciyiGuncelle(okulNumarasi);
                            }
                            else
                            {
                                Console.WriteLine("\nÖğrenci Numarası Sayı Olarak Girilmelidir.");
                            }

                            break;

                        case 6:
                            Console.WriteLine("\nÇıkış Yapıldı");
                            break;

                        default:
                            Console.WriteLine("\nHatalı Seçim");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("\nGiriş Başarısız! Kullanıcı adı veya şifre hatalı.");
            }
        }

        // Kullanıcı giriş bilgilerini doğrulayan fonksiyon
        static bool KullaniciGirisDogrula(string girilenKullaniciAdi, string girilenSifre, string dogruKullaniciAdi, string dogruSifre)
        {
            return (girilenKullaniciAdi == dogruKullaniciAdi && girilenSifre == dogruSifre);
        }

        // Sansürlü şifre girişi için fonksiyon
        static string sifreyiSansurle()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Backspace'e basıldığında bir önceki karakteri sil
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, (password.Length - 1));
                    Console.Write("\b \b");
                }
            }
            while (key.Key != ConsoleKey.Enter);

            return password;
        }
    }

    interface IOgrenci // Ogrenci için metotları tut
    {
        void DevamsizlikKaydet(int devamsizlikSayisi);
    }

    class Ogrenci : IOgrenci
    {
        // kapsülleme get-set metodu
        public string Ad { get; set; }
        public string Soyad { get; set; }

        private int okulNumarasi;

        private int devamsizlikSayisi;

        public int OkulNumarasi
        {
            get { return okulNumarasi; }
            set
            {
                // Okul numarasının 9 haneli olduğunun kontrolü
                if (value.ToString().Length == 9)
                {
                    okulNumarasi = value;
                }
                else
                {
                    Console.WriteLine("\nOkul numarası 9 haneli olmalıdır.");
                }
            }
        }

        public int DevamsizlikSayisi
        {
            get { return devamsizlikSayisi; }
            set
            {
                // Devamsızlık sayısının 0 ile 50 arasında olduğunun kontrolü
                if (value >= 0 && value <= 50)
                {
                    devamsizlikSayisi = value;
                }
                else
                {
                    Console.WriteLine("\nDevamsızlık sayısı 0 ile 50 arasında olmalıdır.");
                }
            }
        }

        // yapıcı blok / constructor
        public Ogrenci(string Ad, string Soyad, int OkulNumarasi)
        {
            this.Ad = Ad;
            this.Soyad = Soyad;
            this.OkulNumarasi = OkulNumarasi;
            this.DevamsizlikSayisi = 0; // devamsızlık sayısı ilk başta 0
        }

        // IOgrenci interface'inden metot al
        public void DevamsizlikKaydet(int devamsizlikSayisi)
        {
            DevamsizlikSayisi += devamsizlikSayisi;
        }
    }

    interface IDevamsizlikIslemleri // DevamsızlıkIslemleri için metotları tut
    {
        void OgrenciEkle(Ogrenci ogrenci);
        void DevamsizlikGirisiYap(int okulNumarasi, int devamsizlikSayisi);
        void OgrencileriListele();
        void OgrenciyiSil(int ogrenciNumarasi);
        void OgrenciyiGuncelle(int okulNumarasi);
    }

    class DevamsizlikIslemleri : IDevamsizlikIslemleri // IDevamsizlikIslemleri interface'inden metotları al
    {
        private List<Ogrenci> ogrenciler; // öğrencileri tutmak için liste

        public DevamsizlikIslemleri()
        {
            ogrenciler = new List<Ogrenci>(); // öğrenci listesini oluştur
            // 2 örnek öğrenci oluştur
            Ogrenci ornekOgrenci1 = new Ogrenci("Yunus Emre", "Karadeniz", 123456788);
            Ogrenci ornekOgrenci2 = new Ogrenci("Sinan", "Elmas", 123456789);
            // listeye örnek öğrencileri ekle
            ogrenciler.Add(ornekOgrenci1);
            ogrenciler.Add(ornekOgrenci2);
        }

        public void OgrenciEkle(Ogrenci ogrenci)
        {
            bool ogrenciDahaOnceEklenmisMi = false; // öğrencinin daha önce eklenip eklenmediğini anlamak için değişken

            foreach (var ogr in ogrenciler) // öğrenciyi eklemeden önce öğrenciler listesini dolaş
            {
                // eğer eklenmek istenen öğrenci adı soyadı başka bir öğrencide varsa
                if (ogr.Ad.Equals(ogrenci.Ad) && ogr.Soyad.Equals(ogrenci.Soyad))
                {
                    Console.WriteLine("\n" + ogr.Ad + " " + ogr.Soyad + " adında bir öğrenci kaydı zaten mevcut !");
                    // uyarı ver
                    ogrenciDahaOnceEklenmisMi = true; // öğrenci daha önce eklenmiştir. TRUE
                    break;
                }
                // eğer eklenmek istenen öğrencinin okul numarası başka bir öğrencide varsa
                else if (ogr.OkulNumarasi == ogrenci.OkulNumarasi)
                {
                    Console.WriteLine("\n" + ogr.OkulNumarasi + " numarasıyla kaydedilmiş bir öğrenci zaten mevcut !");
                    // uyarı ver
                    ogrenciDahaOnceEklenmisMi = true; // öğrenci daha önce eklenmiştir. TRUE
                }
            }

            if (!ogrenciDahaOnceEklenmisMi) // öğrenci daha önce eklenmediyse
            {
                ogrenciler.Add(ogrenci); // listeye verilen öğrenciyi ekle
                Console.Write("\nÖğrenci eklendi:" + ogrenci.Ad + " " + ogrenci.Soyad);
            }
        }

        public void DevamsizlikGirisiYap(int okulNumarasi, int devamsizlikSayisi)
        {
            Ogrenci ogrenci = null; // ogrenci nesnesi oluştur ilk başta değeri yok

            foreach (var ogr in ogrenciler) // verilen okul numarasına sahip öğrenciyi bul
            {
                if (ogr.OkulNumarasi == okulNumarasi)
                {
                    ogrenci = ogr;
                    break;
                }
            }

            if (ogrenci != null) // öğrenci varsa
            {
                ogrenci.DevamsizlikKaydet(devamsizlikSayisi);
                Console.WriteLine($"\n{ogrenci.Ad} {ogrenci.Soyad}'ın devamsızlık kaydı güncellendi. Toplam Devamsızlık: {ogrenci.DevamsizlikSayisi}");
            }
            else // öğrenci yoksa
            {
                Console.WriteLine("\nÖğrenci bulunamadı.");
            }
        }

        public void OgrencileriListele()
        {
            Console.WriteLine("\nÖğrenci Listesi:");
            foreach (var ogrenci in ogrenciler) // ogrenciler listesindeki elemanları yazdır
            {
                Console.WriteLine($"{ogrenci.Ad} {ogrenci.Soyad} - Okul Numarası: {ogrenci.OkulNumarasi} - Toplam Devamsızlık: {ogrenci.DevamsizlikSayisi}");
            }
        }

        public void OgrenciyiSil(int ogrenciNumarasi)
        {
            Ogrenci ogrenci = null; // ogrenci nesnesi oluştur ilk başta değeri yok

            foreach (var ogr in ogrenciler) // verilen okul numarasına sahip öğrenciyi bul
            {
                if (ogr.OkulNumarasi == ogrenciNumarasi)
                {
                    ogrenci = ogr;
                    break;
                }
            }

            if (ogrenci != null) // öğrenci varsa
            {
                Console.WriteLine(ogrenci.Ad + " " + ogrenci.Soyad + " isimli öğrenci silinecektir, emin misiniz? (E (evet)/H (hayır))");
                string secim = Console.ReadLine();

                if (secim.Equals("EVET") || secim.Equals("evet") || secim.Equals("E") || secim.Equals("e"))
                {
                    ogrenciler.Remove(ogrenci);
                    Console.WriteLine("\nÖğrenci Silindi : " + ogrenci.Ad);
                }
                else
                {
                    Console.WriteLine("\nSilme İşlemi İptal Edildi.");
                }
            }
            else // öğrenci yoksa
            {
                Console.WriteLine("\nÖğrenci bulunamadı.");
            }
        }

        public void OgrenciyiGuncelle(int okulNumarasi)
        {
            Ogrenci ogrenci = null; // ogrenci nesnesi oluştur ilk başta değeri yok

            foreach (var ogr in ogrenciler) // verilen okul numarasına sahip öğrenciyi bul
            {
                if (ogr.OkulNumarasi == okulNumarasi)
                {
                    ogrenci = ogr;
                    break;
                }
            }

            if (ogrenci != null)
            {
                Console.WriteLine("\n---Mevcut Öğrenci Bilgileri---");
                Console.WriteLine("Ad: " + ogrenci.Ad);
                Console.WriteLine("Soyad: " + ogrenci.Soyad);
                Console.WriteLine("Okul Numarası: " + ogrenci.OkulNumarasi);
                Console.WriteLine("Devamsızlık Sayısı: " + ogrenci.DevamsizlikSayisi);
                Console.WriteLine("------------------------------");

                string secim = "";

                while (!secim.Equals("3"))
                {
                    Console.WriteLine("\n1-Öğrenci Adını Soyadını Güncelle");
                    Console.WriteLine("2-Öğrencinin Devamsızlığını Güncelle");
                    Console.WriteLine("3-Ana Menüye Dön");
                    Console.Write("Seçim Yap:");

                    secim = Console.ReadLine();

                    switch (secim)
                    {
                        case "1":
                            Console.Write($"\nYeni Öğrenci Adı [eski: {ogrenci.Ad}]: ");
                            string ad = Console.ReadLine();

                            Console.Write($"Yeni Öğrenci Soyadı [eski: {ogrenci.Soyad}]: ");
                            string soyad = Console.ReadLine();

                            foreach (var ogr in ogrenciler)
                            {
                                if (ogr.Ad.Equals(ad) && ogr.Soyad.Equals(soyad))
                                {
                                    Console.WriteLine("\nGirilmeye Çalışılan Ad Soyad Bilgileri Başka Bir Öğrencide Mevcut.");
                                    break;
                                }
                            }

                            ogrenci.Ad = ad;
                            ogrenci.Soyad = soyad;

                            Console.WriteLine("Öğrenci Ad Soyad Bilgileri Başarıyla Güncellendi : " + ogrenci.Ad + " " + ogrenci.Soyad);

                            break;
                        case "2":
                            Console.Write($"\nYeni Devamsızlık Sayısını Girin [eski:{ogrenci.DevamsizlikSayisi}]: ");

                            string devamsizlik = Console.ReadLine();

                            if (int.TryParse(devamsizlik, out int devamsizlikSayisi))
                            {
                                if (devamsizlikSayisi >= 0 && devamsizlikSayisi <= 50)
                                {
                                    ogrenci.DevamsizlikSayisi = devamsizlikSayisi;
                                    Console.WriteLine("Öğrenci Devamsızlık Bilgisi Başarıyla Güncellendi : " + ogrenci.DevamsizlikSayisi);
                                }
                                else
                                {
                                    Console.WriteLine("\nDevamsızlık Sayısı En Az 0 ve En Fazla 50 Olabilir.");
                                    break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nDevamsızlık Sayısı Uygun Şekilde Girilmedi.");
                            }

                            break;
                        case "3":
                            break;
                        default:
                            Console.WriteLine("\nHatalı Seçim, Tekrar Deneyin.");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("\nÖğrenci bulunamadı.");
            }

        }

    }
}