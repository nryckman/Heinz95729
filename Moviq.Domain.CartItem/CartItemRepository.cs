﻿namespace Moviq.Domain.CartItem
{
    using Couchbase;
    using Couchbase.Extensions;
    using Enyim.Caching.Memcached;
    using Moviq.Interfaces.Factories;
    using Moviq.Interfaces.Models;
    using Moviq.Interfaces.Repositories;
    using Newtonsoft.Json;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    public class CartItemRepository : ICartItemRepository<ICartItem>
    {
        protected string keyPattern;
        protected string dataType;

        public CartItemRepository(IFactory<ICartItem> CartItemFactory, ICouchbaseClient db, ILocale locale, IRestClient restClient, string searchUrl)
        {
            this.CartItemFactory = CartItemFactory;
            this.db = db;
            this.locale = locale;
            this.dataType = ((IHelpCategorizeNoSqlData)CartItemFactory.GetInstance())._type;
            this.keyPattern = String.Concat(this.dataType, "::{0}");
            this.restClient = restClient;
            this.searchUrl = searchUrl;
        }

        IFactory<ICartItem> CartItemFactory;
        ICouchbaseClient db;
        ILocale locale;
        IRestClient restClient;
        string searchUrl;
        string query = "{ \"query\": { \"query_string\": { \"query_string\": { \"query\": \"{0}\" } } } }";



        public List<ICartItem> GetCartItems(String lookupByCartId)
        {
            System.Diagnostics.Debug.WriteLine("inside GetCartItems module");
            return db.GetView<CartItem>("carts", "cart_items", true).Stale(StaleMode.False).Key(lookupByCartId).ToList<ICartItem>();          
        }

        public List<ICartItem> GetPurchasedCartItems()
        {
            System.Diagnostics.Debug.WriteLine("inside GetCartItems module");
            return db.GetView<CartItem>("carts", "purchased_cart_items", true).Stale(StaleMode.False).ToList<ICartItem>();
        }

        public ICartItem Get(string uid)
        {
            return db.GetJson<CartItem>(String.Format(keyPattern, uid.ToString()));
        }

        private IEnumerable<ICartItem> Get(IEnumerable<string> keys) 
        {
            if (!keys.Any())
                yield break;

            var _results = db.Get(keys).Where(o => o.Value != null).Select(o => o.Value);

            if (!_results.Any())
                yield break;

            foreach (var o in _results)
                yield return JsonConvert.DeserializeObject<CartItem>(o.ToString());
        }

        public ICartItem Set(ICartItem CartItem)
        {
            // get composite key
            string compositeKeyPattern = "{0}::{1}";
            string compositeKey = String.Format(compositeKeyPattern, CartItem.Guid, CartItem.ProductUid);

            string message = "set composite key: " + compositeKey;
            System.Diagnostics.Debug.WriteLine(message);

            if (db.StoreJson(StoreMode.Set, String.Format(keyPattern, compositeKey), CartItem))
            {
                return Get(compositeKey);
            }

            throw new Exception(locale.CartItemSetFailure);
        }

        public IEnumerable<ICartItem> List(int take, int skip)
        {
            // TODO: We are breaking Liskov Subsitution by not implementing this method!

            // http://localhost:8092/moviq/_design/dev_books/_view/books?stale=false&connection_timeout=60000&limit=20&skip=0
            throw new Exception(locale.LiskovSubstitutionInfraction);
        }

        public async Task<IEnumerable<ICartItem>> Find(string searchFor)
        {
            // alternatively we could use the elasticsearch.NET option
            // http://www.elasticsearch.org/guide/en/elasticsearch/client/net-api/current/_elasticsearch_net.html

            var response = await restClient.ExecutePostTaskAsync(BuildSearchPostRequest(searchFor));
            var result = JsonConvert.DeserializeObject<NoSqlSearchResult>(response.Content);

            if (result.hits == null || result.hits.hits == null || result.hits.hits.Count < 1)
            {
                return null;
            }
            
            List<string> keys = new List<string> { };

            foreach (var item in result.hits.hits)
            {
                keys.Add(item._id);
            }

            return Get(keys);
        }

        public bool Delete(string uid)
        {
            System.Diagnostics.Debug.WriteLine(String.Format(keyPattern, uid));

            
            return db.Remove(String.Format(keyPattern, uid));
        }

        private bool KeyExists(string uid) 
        {
            return db.KeyExists(String.Format(keyPattern, uid));
        }

        /// <summary>
        /// Make the RestRequest object and set appropriate headers and params
        /// </summary>
        /// <param name="url">the url to be used for search</param>
        /// <returns>A RestRequest object</returns>
        private RestRequest BuildSearchPostRequest(string searchFor)
        {
            var request = new RestRequest(searchUrl, Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new
            {
                query = new
                {
                    query_string = new
                    {
                        query_string = new
                        {
                            query = searchFor
                        }
                    }
                }
            });

            return request;

            //var request = new RestRequest(searchUrl, Method.GET);
            //request.AddParameter("q", searchFor);

            //return request;
        }

        public void Dispose()
        {
            // don't dispose the db - it's a singleton
        }
    }
}
