using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PartsWarehouse.Models
{
    public class DostawcyMetadata
    {

        [Required(ErrorMessage = "Podaj nazwę dostawcy")]
        [StringLength(50, ErrorMessage = "Maksymalnie 50 znaków")]
        [Display(Name ="Dostawca")]
        [Remote("nameExist", "Dostawcy", AdditionalFields ="Id_Dostawcy", HttpMethod = "POST", ErrorMessage = "Nazwa już istnieje")]
        public string Nazwa { get; set; }
        [StringLength(50, ErrorMessage = "Maksymalnie 50 znaków")]
        [Display(Name = "Miejscowość")]
        public string Miejscowosc;
        [StringLength(6, ErrorMessage = "Maksymalnie 6 znaków")]
        public string Kod;

    }

    public class DzialyMetadata
    {
        [Required(ErrorMessage = "Podaj nazwę działu")]
        [StringLength(50, ErrorMessage = "Maksymalnie 50 znaków")]
        [Display(Name ="Dział")]
        public string Nazwa;
    }

    public class JMMetadata
    {
        [Required(ErrorMessage = "Podaj jednostkę")]
        [StringLength(50, ErrorMessage = "Maksymalnie 50 znaków")]
        [Display(Name = "Jednostka")]
        public string Nazwa;
    }

    public class KartotekiMetadata
    {
        [Required(ErrorMessage = "Podaj nazwę części")]
        [StringLength(50, ErrorMessage = "Maksymalnie 50 znaków")]
        [Display(Name ="Nazwa części")]
        [Remote("nameExist", "Kartoteki", AdditionalFields ="Id_Kartoteki", HttpMethod = "POST", ErrorMessage = "Nazwa już istnieje")]
        public string Nazwa { get; set; }
        [Required(ErrorMessage = "Podaj stan")]
        public int Stan;
        [Required(ErrorMessage = "Podaj miejsce składowania")]
        [StringLength(50, ErrorMessage = "Maksymalnie 50 znaków")]
        public string Miejsce;
        [Required(ErrorMessage = "Podaj kod kartoteki")]
        [StringLength(50, ErrorMessage = "Maksymalnie 50 znaków")]
        [Remote("codeExist", "Kartoteki",AdditionalFields = "Id_Kartoteki", HttpMethod = "POST", ErrorMessage = "Kod już istnieje")]
        public string Kod { get; set; }
        [Required(ErrorMessage = "Podaj stan ostrzegawczy")]
        [Display(Name = "Niski stan")]
        [Range(1, Int32.MaxValue)]
        public int Niski_Stan;
    }

    public class MPKMetadata
    {
        [Required(ErrorMessage = "Podaj miejsce użycia części")]
        [StringLength(50, ErrorMessage = "Maksymalnie 50 znaków")]
        [Display(Name = "Miejsce")]
        [Remote("nameExist", "MPK", AdditionalFields = "Id_MPK", HttpMethod = "POST", ErrorMessage = "Nazwa już istnieje")]
        public string Nazwa { get; set; }
    }

    public class OsobyMetadata
    {
        [Required(ErrorMessage = "Podaj imię")]
        [StringLength(50, ErrorMessage = "Maksymalnie 50 znaków")]
        [Display(Name = "Imię")]
        public string Imie;
        [Required(ErrorMessage = "Podaj nazwisko")]
        [StringLength(50, ErrorMessage = "Maksymalnie 50 znaków")]
        [Remote("fullNameExist", "Osoby", AdditionalFields = "Imie, Id_Osoby", HttpMethod = "POST", ErrorMessage = "Osoba o takim imieniu i nazwisku już istnieje")]
        public string Nazwisko { get; set; }
    }

    public class PrzyjeciaMetadata
    {
        [Required(ErrorMessage = "Podaj ilość")]
        [Display(Name = "Ilość")]
        public int Ilosc;
        [DataType(DataType.Date)]
        [Display(Name = "Data przyjęcia")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Data_Przyjecia;
    }

    public class WydaniaMetadata
    {
        [Required(ErrorMessage = "Podaj ilość")]
        [Display(Name = "Ilość")]
        [Remote("stanKartoteki", "Wydania", AdditionalFields ="Id_Kartoteki", ErrorMessage = "Przekroczony dostępny stan", HttpMethod = "POST")]
        public int Ilosc { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name ="Data wydania")]
        public Nullable<System.DateTime> Data_Wydania;
        [Required(ErrorMessage = "Wybierz pobierającego")]
        public int Id_Osoby;
        [Required(ErrorMessage = "Wybierz część")]
        public int Id_Kartoteki;
    }

    public class ZamowieniaMetadata
    {
        [Required(ErrorMessage = "Podaj datę")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode =true)]
        [Display(Name = "Data zamówienia")]
        public DateTime Data_zamowienia;
        public bool Realizacja;
        [Display(Name = "Ilość")]
        [Required(ErrorMessage = "Podaj ilość")]
        public Nullable<int> Ilosc;
    }

}