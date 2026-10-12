using System.Text.RegularExpressions;

namespace SafeHeaterSmsAgent;

/// <summary>
/// Live installed packages (tax extra). Never speak $999/$1,099 as the customer total —
/// those are labor components inside the package.
/// Electric 40 $1,199 · Electric 50 $1,299 · Gas 40 $1,449 · Gas 50 $1,499.
/// </summary>
internal static class HeaterPrices
{
    public static string Spoken(bool es, string fuel, string size) => (fuel, size, es) switch
    {
        ("electric", "50", true) => "mil doscientos noventa y nueve dólares",
        ("electric", "50", false) => "one thousand two hundred ninety-nine dollars",
        ("electric", "40", true) => "mil ciento noventa y nueve dólares",
        ("electric", "40", false) => "one thousand one hundred ninety-nine dollars",
        ("gas", "50", true) => "mil cuatrocientos noventa y nueve dólares",
        ("gas", "50", false) => "one thousand four hundred ninety-nine dollars",
        ("gas", "40", true) => "mil cuatrocientos cuarenta y nueve dólares",
        ("gas", "40", false) => "one thousand four hundred forty-nine dollars",
        _ => ""
    };

    public static string Quote(bool es, string fuel, string size, bool objection)
    {
        var lead = objection
            ? (es
                ? "Es el paquete instalado, no el tanque solo. Impuesto aparte. "
                : "That's the full installed package, not the tank alone. Tax extra. ")
            : "";
        var e40 = Spoken(es, "electric", "40");
        var e50 = Spoken(es, "electric", "50");
        var g40 = Spoken(es, "gas", "40");
        var g50 = Spoken(es, "gas", "50");

        if (fuel == "electric" && size == "50")
            return es
                ? lead + $"Eléctrico 50 galones instalado {e50} más impuesto. Incluye tanque e instalación. ¿Le sirve ese, o el de 40 por {e40}?"
                : lead + $"Electric 50-gallon installed is {e50} plus tax. Tank and installation included. Want that, or the 40 at {e40}?";
        if (fuel == "electric" && size == "40")
            return es
                ? lead + $"Eléctrico 40 galones instalado {e40} más impuesto. Incluye tanque e instalación. El de 50 es {e50}. ¿Cuál prefiere?"
                : lead + $"Electric 40-gallon installed is {e40} plus tax. Tank and installation included. The 50 is {e50}. Which do you want?";
        if (fuel == "gas" && size == "50")
            return es
                ? lead + $"Gas 50 galones instalado {g50} más impuesto. Incluye tanque e instalación. ¿Le sirve ese, o el de 40 por {g40}?"
                : lead + $"Gas 50-gallon installed is {g50} plus tax. Tank and installation included. Want that, or the 40 at {g40}?";
        if (fuel == "gas" && size == "40")
            return es
                ? lead + $"Gas 40 galones instalado {g40} más impuesto. Incluye tanque e instalación. El de 50 es {g50}. ¿Cuál prefiere?"
                : lead + $"Gas 40-gallon installed is {g40} plus tax. Tank and installation included. The 50 is {g50}. Which do you want?";
        if (fuel == "electric")
            return es
                ? lead + $"Eléctrico 40 galones {e40} más impuesto, 50 {e50}. Incluye tanque e instalación. ¿Qué tamaño tiene el tanque, 40 o 50?"
                : lead + $"Electric 40-gallon is {e40} plus tax, 50 is {e50}. Tank and installation included. Which size is on the tank, 40 or 50?";
        if (fuel == "gas")
            return es
                ? lead + $"Gas 40 galones {g40} más impuesto, 50 {g50}. Incluye tanque e instalación. ¿Qué tamaño tiene el tanque, 40 o 50?"
                : lead + $"Gas 40-gallon is {g40} plus tax, 50 is {g50}. Tank and installation included. Which size is on the tank, 40 or 50?";
        return es
            ? lead + $"Paquete instalado, impuesto aparte. Eléctrico 40 galones {e40}, 50 {e50}. Gas 40 {g40}, 50 {g50}. Incluye tanque e instalación. ¿El suyo es eléctrico o gas?"
            : lead + $"Installed package, tax extra. Electric 40-gallon is {e40}, 50 is {e50}. Gas 40 is {g40}, 50 is {g50}. Tank and installation included. Is yours electric or gas?";
    }

    public static string ForVoice(string text, bool english)
    {
        var t = text ?? "";
        if (english)
        {
            t = Re(t, @"eleven\s+hundred\s+ninety[-\s]?nine", Spoken(false, "electric", "40"));
            t = Re(t, @"eleven\s+ninety[-\s]?nine", Spoken(false, "electric", "40"));
            t = Re(t, @"twelve\s+ninety[-\s]?nine", Spoken(false, "electric", "50"));
            t = Re(t, @"fourteen\s+forty[-\s]?nine", Spoken(false, "gas", "40"));
            t = Re(t, @"fourteen\s+ninety[-\s]?nine", Spoken(false, "gas", "50"));
            t = Re(t, @"\$?\s*1[,.]?499\b", Spoken(false, "gas", "50"));
            t = Re(t, @"\$?\s*1[,.]?449\b", Spoken(false, "gas", "40"));
            t = Re(t, @"\$?\s*1[,.]?299\b", Spoken(false, "electric", "50"));
            t = Re(t, @"\$?\s*1[,.]?199\b", Spoken(false, "electric", "40"));
        }
        else
        {
            t = Re(t, @"\$?\s*1[,.]?499\b", Spoken(true, "gas", "50"));
            t = Re(t, @"\$?\s*1[,.]?449\b", Spoken(true, "gas", "40"));
            t = Re(t, @"\$?\s*1[,.]?299\b", Spoken(true, "electric", "50"));
            t = Re(t, @"\$?\s*1[,.]?199\b", Spoken(true, "electric", "40"));
        }

        t = Re(t, @"\$999\b", english ? "the labor portion, not the full package" : "la mano de obra, no el paquete");
        t = Re(t, @"\$1[,.]099\b", english ? "gas labor, not the full package" : "mano de obra de gas, no el paquete");
        return t;
    }

    public static string FixSlips(string reply)
    {
        reply ??= "";
        var t = reply.ToLowerInvariant();
        var mentions40 = t.Contains("40-gal") || t.Contains("40 gal") || t.Contains("40 gallon") || t.Contains("40 galones");
        var mentions50 = t.Contains("50-gal") || t.Contains("50 gal") || t.Contains("50 gallon") || t.Contains("50 galones")
            || t.Contains("fifty") || t.Contains("cincuenta");
        var gas = Regex.IsMatch(t, @"\bgas\b") || t.Contains("propano");
        if (mentions50 && !mentions40)
        {
            if (gas)
            {
                reply = Re(reply, @"\$1[,.]199", "$1,499");
                reply = Re(reply, @"\$1199", "$1,499");
                reply = Re(reply, @"\$1[,.]299", "$1,499");
                reply = Re(reply, @"\$1[,.]349", "$1,499");
                reply = Re(reply, @"\$1[,.]449", "$1,499");
                reply = Re(reply, @"eleven\s+ninety[-\s]?nine", "one thousand four hundred ninety-nine");
                reply = Re(reply, @"twelve\s+ninety[-\s]?nine", "one thousand four hundred ninety-nine");
                reply = Re(reply, @"fourteen\s+forty[-\s]?nine", "one thousand four hundred ninety-nine");
                reply = Re(reply, @"mil ciento noventa y nueve", "mil cuatrocientos noventa y nueve");
                reply = Re(reply, @"mil doscientos noventa y nueve", "mil cuatrocientos noventa y nueve");
                reply = Re(reply, @"mil cuatrocientos cuarenta y nueve", "mil cuatrocientos noventa y nueve");
            }
            else
            {
                reply = Re(reply, @"\$1[,.]199", "$1,299");
                reply = Re(reply, @"\$1199", "$1,299");
                reply = Re(reply, @"\$1[,.]349", "$1,299");
                reply = Re(reply, @"eleven\s+ninety[-\s]?nine", "one thousand two hundred ninety-nine");
                reply = Re(reply, @"mil ciento noventa y nueve", "mil doscientos noventa y nueve");
            }
        }

        return reply;
    }

    private static string Re(string text, string pattern, string value) =>
        Regex.Replace(text ?? "", pattern, value, RegexOptions.IgnoreCase);
}
