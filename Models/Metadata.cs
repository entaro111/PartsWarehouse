using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PartsWarehouse.Models
{
    public class DostawcyMetadata
    {
        [Required(ErrorMessage = "Podaj nazwę dostawcy")]
        [StringLength(50, ErrorMessage = "Maksymalnie 50 znaków")]
        public string Nazwa;
        [StringLength(50, ErrorMessage = "Maksymalnie 50 znaków")]
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
        public string Nazwa;
        [Required(ErrorMessage = "Podaj stan")]
        public int Stan;
        [Required(ErrorMessage = "Podaj miejsce składowania")]
        [StringLength(50, ErrorMessage = "Maksymalnie 50 znaków")]
        public string Miejsce;
        [Required(ErrorMessage = "Podaj kod kartoteki")]
        [StringLength(50, ErrorMessage = "Maksymalnie 50 znaków")]
        public string Kod;
    }

    public class MPKMetadata
    {
        [Required(ErrorMessage = "Podaj miejsce użycia części")]
        [StringLength(50, ErrorMessage = "Maksymalnie 50 znaków")]
        [Display(Name = "Miejsce")]
        public string Nazwa;
    }

    public class OsobyMetadata
    {
        [Required(ErrorMessage = "Podaj imię")]
        [StringLength(50, ErrorMessage = "Maksymalnie 50 znaków")]
        public string Imie;
        [Required(ErrorMessage = "Podaj nazwisko")]
        [StringLength(50, ErrorMessage = "Maksymalnie 50 znaków")]
        public string Nazwisko;
    }

    public class PrzyjeciaMetadata
    {
        [Required(ErrorMessage = "Podaj ilość")]
        public int Ilosc;
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Data_Przyjecia;
    }

    public class WydaniaMetadata
    {
        [Required(ErrorMessage = "Podaj ilość")]
        public int Ilosc;
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Data_Wydania;
        [Required(ErrorMessage = "Wybierz osobę")]
        public int Id_Osoby;
        [Required(ErrorMessage = "Wybierz część")]
        public int Id_Kartoteki;
    }

    public class ZamowieniaMetadata
    {
        [Required(ErrorMessage = "Podaj datę")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Data_zamowienia;
        public bool Realizacja;
        [Required(ErrorMessage = "Podaj ilość")]
        public Nullable<int> Ilosc;
    }

}