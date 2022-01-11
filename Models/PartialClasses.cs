using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PartsWarehouse.Models
{
    [MetadataType(typeof(DostawcyMetadata))]
    public partial class Dostawcy
    {
    }

    [MetadataType(typeof(DzialyMetadata))]
    public partial class Dzialy
    {
    }

    [MetadataType(typeof(JMMetadata))]
    public partial class JM
    {
    }

    [MetadataType(typeof(KartotekiMetadata))]
    public partial class Kartoteki
    {
    }

    [MetadataType(typeof(MPKMetadata))]
    public partial class MPK
    {
    }

    [MetadataType(typeof(OsobyMetadata))]
    public partial class Osoby
    {
    }

    [MetadataType(typeof(PrzyjeciaMetadata))]
    public partial class Przyjecia
    {
    }

    [MetadataType(typeof(WydaniaMetadata))]
    public partial class Wydania
    {
       
    }

    [MetadataType(typeof(ZamowieniaMetadata))]
    public partial class Zamowienia
    {
    }
}