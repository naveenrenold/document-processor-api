using System.ComponentModel.DataAnnotations;

namespace DocumentProcessor.Model
{
    public class BaseFilter
    {
        /// <summary>
        /// BaseFilter constructor
        /// </summary>
        public BaseFilter(string orderBy)
        {
            OrderBy ??= orderBy;            
        }
        /// <summary>
        /// Defaule value is 25 records max
        /// For pagination
        /// </summary>
        //[BindProperty(Name = "limit")]
        public int? Limit { get; set; } = 25;
        /// <summary>
        /// Default value is 0. Typically this will skip top offset records
        /// </summary>
        //[BindProperty(Name = "offset")]
        public int? Offset { get; set; }
        /// <summary>
        /// Supports filter/refine the result
        /// Format : feild[space]operator[space]value
        /// Operators : eq, ne, lt, gt, le, ge, in, lk, bt | [eq equals][ne notequal][lt lesthan][gt greaterthan][lte lessthanOrequal][gte greaterthanOrequal][in inlist][lk like][bt between]
        /// Example: gender eq male and cityId in uk,us,in and name lk david -> gender=male and cityId in (uk,us,in) and name like '%david%'
        /// Between: ex., age bt 25|45 which is equal to age lessthan or equal to 25 and greaterthan equalto 45
        /// </summary>
        //[BindProperty(Name = "q")]
        public string? Query { get; set; }
        /// <summary>
        /// Feild that needs to be shorted
        /// </summary>
        //[BindProperty(Name = "order")]
        public string? OrderBy { get; set; }
        /// <summary>
        /// Sorting order
        /// Possible values : asc or desc
        /// </summary>
        //[BindProperty(Name = "sort")]
        public string? SortBy { get; set; } = "asc";
        /// <summary>
        /// Limit the response column
        /// Ex., field1,field2
        /// Results rest of the column will be ignored
        /// </summary>
        //[BindProperty(Name = "fields")]
        public virtual string? Field { get; set; }
        ///<summary>
        ///List of filter class
        ///</summary>
        //[JsonIgnore]
        public List<Filter>? Filters { get; set; }
        public string? DefaultField { get; set; }

        ///<summary>
        ///Validate method to validate the properties
        ///</summary>
        public IEnumerable<ValidationResult> Validate(Type type)
        {
            var sorting = new string[] { "asc", "desc" };
            List<ValidationResult> validationError = new List<ValidationResult>();
            _ = ValidateQuery(type, Query, ref validationError);
            _ = ValidateField(type, Field, ref validationError);
            if (!sorting.Contains(SortBy?.ToLower()))
            {
                validationError.Add(new ValidationResult($"Provided value is not valid {OrderBy}.", new[] { nameof(OrderBy) }));
            }
            return validationError;
        }

        public static readonly List<string> applicationOperator = new List<string> {
        "eq",
        "ne",
        "lt",
        "gt",
        "le",
        "ge",
        "in",
        "ni",
        "lk",
        "bt"
        };

        public static bool ValidateQuery(Type t, string? query, ref List<ValidationResult> errors)
        {
            if (string.IsNullOrEmpty(query))
            {
                return true;
            }

            IEnumerable<string> conditions = query.Split(" and ").Select(s => s.Trim());
            IEnumerable<string> invalidFilter = conditions.Where(w => w.Split(" ").Length < 3);
            if (invalidFilter.Any())
            {
                errors.Add(new ValidationResult("Invalid query", invalidFilter));
                return false;
            }
            List<string> selector = t.GetProperties().Select(s => s.Name.ToLower()).ToList();            
            IEnumerable<string> invalidSelector = conditions.Where(w => !selector.Contains(w.Split(" ").FirstOrDefault().ToLower().Trim()));
            if (invalidSelector.Any())
            {
                errors.Add(new ValidationResult("Invalid query", invalidSelector));
                return false;
            }
            IEnumerable<string> invalidOperator = conditions.Where(w => !applicationOperator.Contains(w.Split(" ").Skip(1).FirstOrDefault().ToLower().Trim()));
            if (invalidOperator.Any())
            {
                errors.Add(new ValidationResult("Invalid query", invalidOperator));
                return false;
            }
            return true;
        }
        public static bool ValidateField(Type t, string? feild, ref List<ValidationResult> errors)
        {
            if (string.IsNullOrEmpty(feild))
            {
                return true;
            }

            List<string> selector = t.GetProperties().Select(s => s.Name.ToLower()).ToList();
            IEnumerable<string> invalidSelector = feild.Split(',').Where(w => !selector.Contains(w.ToLower().Trim()));
            if (invalidSelector.Any())
            {
                errors.Add(new ValidationResult("Invalid query", invalidSelector));
                return false;
            }
            return true;
        }
    }
    ///<summary>
    ///Filter class
    ///</summary>
    public class Filter
    {
        ///<summary>
        ///Setting the target
        ///</summary>
        public required string Target { get; set; }
        ///<summary>
        ///Setting the operator
        ///</summary>
        public required string Operator { get; set; }
        ///<summary>
        ///RHS value
        ///</summary>
        public required string Value { get; set; }
    }     
    }
