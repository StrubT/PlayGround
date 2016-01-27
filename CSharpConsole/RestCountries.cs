using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using c = System.Console;

namespace StrubT.PlayGround.CSharpConsole {

	public class RestCountries : IRunnable {

		public bool Active => false;

		public void Run() {

			var col = c.ForegroundColor;
			var key = File.ReadAllText("C:\\temp\\MashapeKey.txt");

			using (var web = new WebClient()) {

				web.BaseAddress = "https://restcountries-v1.p.mashape.com/";
				web.Headers.Add(HttpRequestHeader.Accept, "application/json");
				web.Headers.Add("X-Mashape-Key", key);

				using (var rdr = new StreamReader(web.OpenRead("all"), Encoding.UTF8))
					foreach (var cty in from c in JArray.Parse(rdr.ReadToEnd())
															orderby c.Value<string>("alpha2Code")
															select new Country {
																CodeAlpha2 = c.Value<string>("alpha2Code"),
																CodeAlpha3 = c.Value<string>("alpha3Code"),
																NameEnglish = c.Value<string>("name"),
																NameNative = c.Value<string>("nativeName"),
																NamesAlternativeRaw = c.Value<JArray>("altSpellings").Values<string>().ToList(),
																NamesTranslatedRaw = c.Value<JObject>("translations").Properties().ToDictionary(t => t.Name, t => (string)t.Value),
																Capital = c.Value<string>("capital").NullIf(string.IsNullOrEmpty),
																Demonym = c.Value<string>("demonym").NullIf(string.IsNullOrEmpty),
																Region = c.Value<string>("region").NullIf(string.IsNullOrEmpty),
																SubRegion = c.Value<string>("subregion"),
																//Relevance = c.Value<string>("relevance"),
																Population = c.Value<int?>("population"),
																Area = c.Value<double?>("area"),
																Gini = c.Value<double?>("gini"),
																Location = c.Value<JArray>("latlng")?.Count == 2 ? new GeoCoordinate(c.Value<JArray>("latlng").Value<double>(0), c.Value<JArray>("latlng").Value<double>(1)) : GeoCoordinate.Unknown,
																SharesBorderWithCodesAlpha3 = (ICollection<string>)c.Value<JArray>("borders")?.Values<string>().ToList() ?? new string[] { },
																LanguagesCodesAlpha2 = (ICollection<string>)c.Value<JArray>("languages")?.Values<string>().ToList() ?? new string[] { },
																TimeZonesCodes = (ICollection<string>)c.Value<JArray>("timezones")?.Values<string>().ToList() ?? new string[] { },
																CallingCodes = (ICollection<string>)c.Value<JArray>("callingCodes")?.Values<string>().ToList() ?? new string[] { },
																TopLevelDomains = (ICollection<string>)c.Value<JArray>("topLevelDomain")?.Values<string>().ToList() ?? new string[] { },
																Currencies = (ICollection<string>)c.Value<JArray>("currencies")?.Values<string>().ToList() ?? new string[] { },
															})
						Country.Lookup.Add(cty);

				WriteRegionTree();
				c.WriteLine();
				c.WriteLine();

				WriteCountry(Country.Lookup.Single(c => c.CodeAlpha2 == "CH"));
				c.WriteLine();
				WriteCountry(Country.Lookup.Single(c => c.CodeAlpha2 == "GB"));
				c.WriteLine();
				WriteCountry(Country.Lookup.Single(c => c.CodeAlpha2 == "US"));
				c.WriteLine();
				WriteCountry(Country.Lookup.Single(c => c.CodeAlpha2 == "AU"));
				c.WriteLine();
				WriteCountry(Country.Lookup.Single(c => c.CodeAlpha2 == "UM"));
			}

			c.ForegroundColor = col;
		}

		private void WriteRegionTree() {

			c.ForegroundColor = ConsoleColor.Gray;
			foreach (var reg in from c in Country.Lookup
													where c.Region != null
													group c by c.Region into r
													orderby r.Key
													select r) {
				c.WriteLine($">{reg.Key}");

				foreach (var sub in from c in reg
														where c.SubRegion != null
														group c by c.SubRegion into s
														orderby s.Key
														select s) {
					c.WriteLine($"->{sub.Key}");

					c.ForegroundColor = ConsoleColor.DarkGray;
					c.WriteLine("-->{0}", string.Join(", ", from c in sub
																									orderby c.NameEnglish
																									select c.NameEnglish));
					c.ForegroundColor = ConsoleColor.Gray;
				}
			}
		}

		private void WriteCountry(Country country) {

			c.WriteLine($"*** {country.NameEnglish} ***");
			c.WriteLine($"ISO codes: {country.CodeAlpha2} / {country.CodeAlpha3}");
			c.WriteLine($"Native name: {country.NameNative}");
			c.WriteLine($"Alternative name(s): {string.Join(" / ", country.NamesAlternative)}");
			c.WriteLine($"Translated name(s): {string.Join(", ", country.NamesTranslated.Select(t => $"{t.Key}: {t.Value}"))}");
			c.WriteLine($"Capital: {country.Capital ?? "n/a"}");
			c.WriteLine($"Demonym: {country.Demonym ?? "n/a"}");
			c.WriteLine($"Region: {country.Region ?? "n/a"}");
			c.WriteLine($"Sub-region: {country.SubRegion ?? "n/a"}");
			//c.WriteLine($"Relevance: {country.Relevance}");
			c.WriteLine($"Population: {country.Population ?? 0:#,##0;;n/a}");
			c.WriteLine($"Area: {country.Area ?? 0:#,##0.## km^2;;n/a}");
			c.WriteLine($"Population: {country.PopulationDensity ?? 0:#,##0.## km^-2;;n/a}");
			c.WriteLine($"Gini co-efficient: {country.Gini ?? 0:0.0;;n/a}");
			c.WriteLine("Location: " + (!country.Location.IsUnknown ? $"{country.Location.Latitude:0.0°} {country.Location.Longitude:0.0°}" : "n/a"));
			c.WriteLine($"Shares border(s) with: {string.Join(", ", country.SharesBorderWith.Select(c => c.NameEnglish).DefaultIfEmpty("none"))}");
			c.WriteLine($"Language(s): {string.Join(", ", country.LanguagesCodesAlpha2.DefaultIfEmpty("none"))}");
			c.WriteLine($"Time zone(s): {string.Join(", ", country.TimeZonesUtcOffset.Select(z => $"{z.Hours:+00;-00;00}{z:\\hmm}").DefaultIfEmpty("none"))} UTF offset");
			c.WriteLine($"Calling code(s): {string.Join(", ", country.CallingCodes.DefaultIfEmpty("none"))}");
			c.WriteLine($"Internet-TLD(s): {string.Join(", ", country.TopLevelDomains.DefaultIfEmpty("none"))}");
			c.WriteLine($"Currency/ies: {string.Join(", ", country.Currencies.DefaultIfEmpty("none"))}");
		}

		private class Country {

			public string CodeAlpha2 { get; set; }

			public string CodeAlpha3 { get; set; }

			public string NameEnglish { get; set; }

			public string NameNative { get; set; }

			internal ICollection<string> NamesAlternativeRaw { get; set; }

			public ICollection<string> NamesAlternative => NamesAlternativeRaw.Except(new[] { CodeAlpha2, CodeAlpha3, NameEnglish, NameNative }).ToList();

			internal IDictionary<string, string> NamesTranslatedRaw { get; set; }

			public IDictionary<string, string> NamesTranslated => NamesTranslatedRaw.Union(new[] { new KeyValuePair<string, string>("en", NameEnglish) }.Where(p => !NamesTranslatedRaw.ContainsKey(p.Key))).ToDictionary(p => p.Key, p => p.Value);

			public string Capital { get; set; }

			public string Demonym { get; set; }

			public string Region { get; set; }

			public string SubRegion { get; set; }

			//public string Relevance { get; set; }

			public int? Population { get; set; }

			public double? Area { get; set; } //km^2

			public double? PopulationDensity => Population / Area;

			public double? Gini { get; set; }

			public GeoCoordinate Location { get; set; }

			public double? Latitude => !Location.IsUnknown ? (double?)Location.Latitude : null;

			public double? Longitude => !Location.IsUnknown ? (double?)Location.Longitude : null;

			public ICollection<string> SharesBorderWithCodesAlpha3 { get; set; }

			public ICollection<Country> SharesBorderWith => SharesBorderWithCodesAlpha3.Join(Lookup, a => a, c => c.CodeAlpha3, (a, c) => c).ToList();

			public ICollection<string> LanguagesCodesAlpha2 { get; set; }

			public ICollection<string> TimeZonesCodes { get; set; }

			public ICollection<TimeSpan> TimeZonesUtcOffset => TimeZonesCodes.Select(ConvertTimeZoneCodeToUtcOffset).ToList();

			public ICollection<string> CallingCodes { get; set; }

			public ICollection<string> TopLevelDomains { get; set; }

			public ICollection<string> Currencies { get; set; }

			public override int GetHashCode() => 13 + CodeAlpha3.GetHashCode();

			public override bool Equals(object obj) => obj != null && GetType().Equals(obj.GetType()) && CodeAlpha3.Equals(((Country)obj).CodeAlpha3);

			public override string ToString() => $"{CodeAlpha2} / {CodeAlpha3}: {NameEnglish} ({NameNative})";

			public static TimeSpan ConvertTimeZoneCodeToUtcOffset(string input) {

				if (input == "UTC") return TimeSpan.Zero;
				if (input.Length != 9 || input.Substring(0, 3) != "UTC") throw new FormatException("The time zone code is not in the valid format 'UTC[+-]##:##'.");

				var zone = TimeSpan.ParseExact(input.Substring(4), "hh\\:mm", CultureInfo.InvariantCulture);
				if (input[3] != '+') zone = zone.Negate();
				return zone;
			}

			internal static ICollection<Country> Lookup { get; set; } = new List<Country>();
		}
	}
}
