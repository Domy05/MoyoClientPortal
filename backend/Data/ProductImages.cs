namespace ClientPortal.Api.Data;

public record ProductImageVariant(string Brand, string ImageUrl);

public static class ProductImages
{
    public static readonly Dictionary<string, ProductImageVariant[]> All = new()
    {
        // =========================================================
        // STATIONERY
        // =========================================================

        ["ballpoint-pen"] = new[]
        {
            new ProductImageVariant("Bic", "https://images.pexels.com/photos/15927885/pexels-photo-15927885.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Pilot", "https://images.pexels.com/photos/15927885/pexels-photo-15927885.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Parker", "https://images.pexels.com/photos/15927885/pexels-photo-15927885.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Staedtler", "https://images.pexels.com/photos/15927885/pexels-photo-15927885.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Uni-ball", "https://images.pexels.com/photos/15927885/pexels-photo-15927885.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Pentel", "https://images.pexels.com/photos/15927885/pexels-photo-15927885.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Paper Mate", "https://images.pexels.com/photos/15927885/pexels-photo-15927885.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Schneider", "https://images.pexels.com/photos/15927885/pexels-photo-15927885.jpeg?auto=compress&cs=tinysrgb&w=800"),
        },

        ["notebook"] = new[]
        {
            new ProductImageVariant("Moleskine", "https://images.pexels.com/photos/7718658/pexels-photo-7718658.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Croxley", "https://images.pexels.com/photos/7718658/pexels-photo-7718658.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Bantex", "https://images.pexels.com/photos/7718658/pexels-photo-7718658.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Typo", "https://images.pexels.com/photos/7718658/pexels-photo-7718658.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Leuchtturm1917", "https://images.pexels.com/photos/7718658/pexels-photo-7718658.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Clairefontaine", "https://images.pexels.com/photos/7718658/pexels-photo-7718658.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Oxford", "https://images.pexels.com/photos/7718658/pexels-photo-7718658.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Fabriano", "https://images.pexels.com/photos/7718658/pexels-photo-7718658.jpeg?auto=compress&cs=tinysrgb&w=800"),
        },

        ["gel-pens"] = new[]
        {
            new ProductImageVariant("Pilot", "https://images.pexels.com/photos/7718658/pexels-photo-7718658.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Uni-ball", "https://images.pexels.com/photos/7718658/pexels-photo-7718658.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Pentel", "https://images.pexels.com/photos/7718658/pexels-photo-7718658.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Stabilo", "https://images.pexels.com/photos/7718658/pexels-photo-7718658.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Paper Mate", "https://images.pexels.com/photos/7718658/pexels-photo-7718658.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Zebra", "https://images.pexels.com/photos/7718658/pexels-photo-7718658.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Schneider", "https://images.pexels.com/photos/7718658/pexels-photo-7718658.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Faber-Castell", "https://images.pexels.com/photos/7718658/pexels-photo-7718658.jpeg?auto=compress&cs=tinysrgb&w=800"),
        },

        ["sticky-notes"] = new[]
        {
            new ProductImageVariant("Post-it", "https://images.pexels.com/photos/8071651/pexels-photo-8071651.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Croxley", "https://images.pexels.com/photos/8071651/pexels-photo-8071651.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Stick'n", "https://images.pexels.com/photos/8071651/pexels-photo-8071651.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Bantex", "https://images.pexels.com/photos/8071651/pexels-photo-8071651.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("3M", "https://images.pexels.com/photos/8071651/pexels-photo-8071651.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Hopax", "https://images.pexels.com/photos/8071651/pexels-photo-8071651.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Oxford", "https://images.pexels.com/photos/8071651/pexels-photo-8071651.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Leitz", "https://images.pexels.com/photos/8071651/pexels-photo-8071651.jpeg?auto=compress&cs=tinysrgb&w=800"),
        },

        ["paper-clips"] = new[]
        {
            new ProductImageVariant("Bantex", "https://images.pexels.com/photos/7718863/pexels-photo-7718863.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Croxley", "https://images.pexels.com/photos/7718863/pexels-photo-7718863.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Rexel", "https://images.pexels.com/photos/7718863/pexels-photo-7718863.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Officemate", "https://images.pexels.com/photos/7718863/pexels-photo-7718863.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Deli", "https://images.pexels.com/photos/7718863/pexels-photo-7718863.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Maped", "https://images.pexels.com/photos/7718863/pexels-photo-7718863.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Kangaro", "https://images.pexels.com/photos/7718863/pexels-photo-7718863.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Leitz", "https://images.pexels.com/photos/7718863/pexels-photo-7718863.jpeg?auto=compress&cs=tinysrgb&w=800"),
        },

        ["whiteboard-markers"] = new[]
        {
            new ProductImageVariant("Staedtler", "https://images.pexels.com/photos/7173040/pexels-photo-7173040.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Pentel", "https://images.pexels.com/photos/7173040/pexels-photo-7173040.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Artline", "https://images.pexels.com/photos/7173040/pexels-photo-7173040.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Bantex", "https://images.pexels.com/photos/7173040/pexels-photo-7173040.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Pilot", "https://images.pexels.com/photos/7173040/pexels-photo-7173040.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Faber-Castell", "https://images.pexels.com/photos/7173040/pexels-photo-7173040.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Schneider", "https://images.pexels.com/photos/7173040/pexels-photo-7173040.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Edding", "https://images.pexels.com/photos/7173040/pexels-photo-7173040.jpeg?auto=compress&cs=tinysrgb&w=800"),
        },

        // =========================================================
        // PAPER
        // =========================================================

        ["copy-paper"] = new[]
        {
            new ProductImageVariant("Typek", "https://images.pexels.com/photos/6368842/pexels-photo-6368842.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Double A", "https://images.pexels.com/photos/6368842/pexels-photo-6368842.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Xerox", "https://images.pexels.com/photos/6368842/pexels-photo-6368842.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("HP", "https://images.pexels.com/photos/6368842/pexels-photo-6368842.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Navigator", "https://images.pexels.com/photos/6368842/pexels-photo-6368842.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Mondi", "https://images.pexels.com/photos/6368842/pexels-photo-6368842.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Rotatrim", "https://images.pexels.com/photos/6368842/pexels-photo-6368842.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("PaperOne", "https://images.pexels.com/photos/6368842/pexels-photo-6368842.jpeg?auto=compress&cs=tinysrgb&w=800"),
        },

        // =========================================================
        // OFFICE
        // =========================================================

        ["office-printer"] = new[]
        {
            new ProductImageVariant("HP", "https://images.unsplash.com/photo-1612815154858-60aa4c59eaa6?auto=format&fit=crop&w=800&q=80"),
            new ProductImageVariant("Canon", "https://images.unsplash.com/photo-1612815154858-60aa4c59eaa6?auto=format&fit=crop&w=800&q=80"),
            new ProductImageVariant("Epson", "https://images.unsplash.com/photo-1612815154858-60aa4c59eaa6?auto=format&fit=crop&w=800&q=80"),
            new ProductImageVariant("Brother", "https://images.unsplash.com/photo-1612815154858-60aa4c59eaa6?auto=format&fit=crop&w=800&q=80"),
            new ProductImageVariant("Samsung", "https://images.unsplash.com/photo-1612815154858-60aa4c59eaa6?auto=format&fit=crop&w=800&q=80"),
            new ProductImageVariant("Xerox", "https://images.unsplash.com/photo-1612815154858-60aa4c59eaa6?auto=format&fit=crop&w=800&q=80"),
            new ProductImageVariant("Lexmark", "https://images.unsplash.com/photo-1612815154858-60aa4c59eaa6?auto=format&fit=crop&w=800&q=80"),
            new ProductImageVariant("Kyocera", "https://images.unsplash.com/photo-1612815154858-60aa4c59eaa6?auto=format&fit=crop&w=800&q=80"),
        },

        ["stapler"] = new[]
        {
            new ProductImageVariant("Bostitch", "https://images.unsplash.com/photo-1562966700-49bb28f1c62d?auto=format&fit=crop&w=800&q=80"),
            new ProductImageVariant("Rexel", "https://images.unsplash.com/photo-1562966700-49bb28f1c62d?auto=format&fit=crop&w=800&q=80"),
            new ProductImageVariant("Kangaro", "https://images.unsplash.com/photo-1562966700-49bb28f1c62d?auto=format&fit=crop&w=800&q=80"),
            new ProductImageVariant("Deli", "https://images.unsplash.com/photo-1562966700-49bb28f1c62d?auto=format&fit=crop&w=800&q=80"),
            new ProductImageVariant("Leitz", "https://images.unsplash.com/photo-1562966700-49bb28f1c62d?auto=format&fit=crop&w=800&q=80"),
            new ProductImageVariant("Rapid", "https://images.unsplash.com/photo-1562966700-49bb28f1c62d?auto=format&fit=crop&w=800&q=80"),
            new ProductImageVariant("Maped", "https://images.unsplash.com/photo-1562966700-49bb28f1c62d?auto=format&fit=crop&w=800&q=80"),
            new ProductImageVariant("Novus", "https://images.unsplash.com/photo-1562966700-49bb28f1c62d?auto=format&fit=crop&w=800&q=80"),
        },

        ["file-folders"] = new[]
        {
            new ProductImageVariant("Bantex", "https://images.pexels.com/photos/6368842/pexels-photo-6368842.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Croxley", "https://images.pexels.com/photos/6368842/pexels-photo-6368842.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Esselte", "https://images.pexels.com/photos/6368842/pexels-photo-6368842.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Rexel", "https://images.pexels.com/photos/6368842/pexels-photo-6368842.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Leitz", "https://images.pexels.com/photos/6368842/pexels-photo-6368842.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Fellowes", "https://images.pexels.com/photos/6368842/pexels-photo-6368842.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Elba", "https://images.pexels.com/photos/6368842/pexels-photo-6368842.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Oxford", "https://images.pexels.com/photos/6368842/pexels-photo-6368842.jpeg?auto=compress&cs=tinysrgb&w=800"),
        },

        ["scissors"] = new[]
        {
            new ProductImageVariant("Fiskars", "https://images.pexels.com/photos/8125680/pexels-photo-8125680.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Bantex", "https://images.pexels.com/photos/8125680/pexels-photo-8125680.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Maped", "https://images.pexels.com/photos/8125680/pexels-photo-8125680.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Westcott", "https://images.pexels.com/photos/8125680/pexels-photo-8125680.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Kangaro", "https://images.pexels.com/photos/8125680/pexels-photo-8125680.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Staedtler", "https://images.pexels.com/photos/8125680/pexels-photo-8125680.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Faber-Castell", "https://images.pexels.com/photos/8125680/pexels-photo-8125680.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Leitz", "https://images.pexels.com/photos/8125680/pexels-photo-8125680.jpeg?auto=compress&cs=tinysrgb&w=800"),
        },

        ["calculator"] = new[]
        {
            new ProductImageVariant("Casio", "https://images.pexels.com/photos/7688365/pexels-photo-7688365.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Sharp", "https://images.pexels.com/photos/7688365/pexels-photo-7688365.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("HP", "https://images.pexels.com/photos/7688365/pexels-photo-7688365.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Citizen", "https://images.pexels.com/photos/7688365/pexels-photo-7688365.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Texas Instruments", "https://images.pexels.com/photos/7688365/pexels-photo-7688365.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Canon", "https://images.pexels.com/photos/7688365/pexels-photo-7688365.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Aurora", "https://images.pexels.com/photos/7688365/pexels-photo-7688365.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Olympia", "https://images.pexels.com/photos/7688365/pexels-photo-7688365.jpeg?auto=compress&cs=tinysrgb&w=800"),
        },

        ["desk-organiser"] = new[]
        {
            new ProductImageVariant("Bantex", "https://images.pexels.com/photos/31512989/pexels-photo-31512989.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Deflecto", "https://images.pexels.com/photos/31512989/pexels-photo-31512989.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Leitz", "https://images.pexels.com/photos/31512989/pexels-photo-31512989.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Croxley", "https://images.pexels.com/photos/31512989/pexels-photo-31512989.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Fellowes", "https://images.pexels.com/photos/31512989/pexels-photo-31512989.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Deli", "https://images.pexels.com/photos/31512989/pexels-photo-31512989.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Maped", "https://images.pexels.com/photos/31512989/pexels-photo-31512989.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Rotring", "https://images.pexels.com/photos/31512989/pexels-photo-31512989.jpeg?auto=compress&cs=tinysrgb&w=800"),
        },

        // =========================================================
        // CLEANING
        // =========================================================

        ["surface-cleaner"] = new[]
        {
            new ProductImageVariant("Dettol", "https://images.pexels.com/photos/8348922/pexels-photo-8348922.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Handy Andy", "https://images.pexels.com/photos/8348922/pexels-photo-8348922.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Domestos", "https://images.pexels.com/photos/8348922/pexels-photo-8348922.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Cillit Bang", "https://images.pexels.com/photos/8348922/pexels-photo-8348922.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Mr Muscle", "https://images.pexels.com/photos/8348922/pexels-photo-8348922.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Jik", "https://images.pexels.com/photos/8348922/pexels-photo-8348922.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Sunlight", "https://images.pexels.com/photos/8348922/pexels-photo-8348922.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Astonish", "https://images.pexels.com/photos/8348922/pexels-photo-8348922.jpeg?auto=compress&cs=tinysrgb&w=800"),
        },

        ["disinfectant-wipes"] = new[]
        {
            new ProductImageVariant("Dettol", "https://images.unsplash.com/photo-1611762820957-d81b7aaa7d42?auto=format&fit=crop&w=800&q=80"),
            new ProductImageVariant("Clorox", "https://images.unsplash.com/photo-1611762820957-d81b7aaa7d42?auto=format&fit=crop&w=800&q=80"),
            new ProductImageVariant("Lysol", "https://images.unsplash.com/photo-1611762820957-d81b7aaa7d42?auto=format&fit=crop&w=800&q=80"),
            new ProductImageVariant("Sunlight", "https://images.unsplash.com/photo-1611762820957-d81b7aaa7d42?auto=format&fit=crop&w=800&q=80"),
            new ProductImageVariant("Jik", "https://images.unsplash.com/photo-1611762820957-d81b7aaa7d42?auto=format&fit=crop&w=800&q=80"),
            new ProductImageVariant("Handy Andy", "https://images.unsplash.com/photo-1611762820957-d81b7aaa7d42?auto=format&fit=crop&w=800&q=80"),
            new ProductImageVariant("Mr Muscle", "https://images.unsplash.com/photo-1611762820957-d81b7aaa7d42?auto=format&fit=crop&w=800&q=80"),
            new ProductImageVariant("Shield", "https://images.unsplash.com/photo-1611762820957-d81b7aaa7d42?auto=format&fit=crop&w=800&q=80"),
        },

        ["facial-tissues"] = new[]
        {
            new ProductImageVariant("Kleenex", "https://images.pexels.com/photos/3957980/pexels-photo-3957980.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Softex", "https://images.pexels.com/photos/3957980/pexels-photo-3957980.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Twinsaver", "https://images.pexels.com/photos/3957980/pexels-photo-3957980.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Baby Soft", "https://images.pexels.com/photos/3957980/pexels-photo-3957980.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Scott", "https://images.pexels.com/photos/3957980/pexels-photo-3957980.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Cusheen", "https://images.pexels.com/photos/3957980/pexels-photo-3957980.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Paseo", "https://images.pexels.com/photos/3957980/pexels-photo-3957980.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Regina", "https://images.pexels.com/photos/3957980/pexels-photo-3957980.jpeg?auto=compress&cs=tinysrgb&w=800"),
        },

        // =========================================================
        // PANTRY
        // =========================================================

        ["coffee"] = new[]
        {
            new ProductImageVariant("Ricoffy", "https://images.pexels.com/photos/22588872/pexels-photo-22588872.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Nescafé", "https://images.pexels.com/photos/22588872/pexels-photo-22588872.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Frisco", "https://images.pexels.com/photos/22588872/pexels-photo-22588872.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Jacobs", "https://images.pexels.com/photos/22588872/pexels-photo-22588872.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Douwe Egberts", "https://images.pexels.com/photos/22588872/pexels-photo-22588872.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Lavazza", "https://images.pexels.com/photos/22588872/pexels-photo-22588872.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Seattle Coffee Co.", "https://images.pexels.com/photos/22588872/pexels-photo-22588872.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Vida e Caffè", "https://images.pexels.com/photos/22588872/pexels-photo-22588872.jpeg?auto=compress&cs=tinysrgb&w=800"),
        },

        ["tea-bags"] = new[]
        {
            new ProductImageVariant("Five Roses", "https://images.pexels.com/photos/1417945/pexels-photo-1417945.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Joko", "https://images.pexels.com/photos/1417945/pexels-photo-1417945.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Freshpak", "https://images.pexels.com/photos/1417945/pexels-photo-1417945.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Twinings", "https://images.pexels.com/photos/1417945/pexels-photo-1417945.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Lipton", "https://images.pexels.com/photos/1417945/pexels-photo-1417945.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Tetley", "https://images.pexels.com/photos/1417945/pexels-photo-1417945.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Rooibos Ltd", "https://images.pexels.com/photos/1417945/pexels-photo-1417945.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Dilmah", "https://images.pexels.com/photos/1417945/pexels-photo-1417945.jpeg?auto=compress&cs=tinysrgb&w=800"),
        },

        ["sugar"] = new[]
        {
            new ProductImageVariant("Selati", "https://images.pexels.com/photos/13800738/pexels-photo-13800738.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Huletts", "https://images.pexels.com/photos/13800738/pexels-photo-13800738.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Illovo", "https://images.pexels.com/photos/13800738/pexels-photo-13800738.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Sugar Valley", "https://images.pexels.com/photos/13800738/pexels-photo-13800738.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Sasko", "https://images.pexels.com/photos/13800738/pexels-photo-13800738.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Natura", "https://images.pexels.com/photos/13800738/pexels-photo-13800738.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Billington's", "https://images.pexels.com/photos/13800738/pexels-photo-13800738.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Tate & Lyle", "https://images.pexels.com/photos/13800738/pexels-photo-13800738.jpeg?auto=compress&cs=tinysrgb&w=800"),
        },

        // =========================================================
        // PRINTING
        // =========================================================

        ["printer-ink"] = new[]
        {
            new ProductImageVariant("HP", "https://images.pexels.com/photos/4792285/pexels-photo-4792285.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Canon", "https://images.pexels.com/photos/4792285/pexels-photo-4792285.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Epson", "https://images.pexels.com/photos/4792285/pexels-photo-4792285.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Brother", "https://images.pexels.com/photos/4792285/pexels-photo-4792285.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Lexmark", "https://images.pexels.com/photos/4792285/pexels-photo-4792285.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Samsung", "https://images.pexels.com/photos/4792285/pexels-photo-4792285.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Xerox", "https://images.pexels.com/photos/4792285/pexels-photo-4792285.jpeg?auto=compress&cs=tinysrgb&w=800"),
            new ProductImageVariant("Kodak", "https://images.pexels.com/photos/4792285/pexels-photo-4792285.jpeg?auto=compress&cs=tinysrgb&w=800"),
        },
    };
}