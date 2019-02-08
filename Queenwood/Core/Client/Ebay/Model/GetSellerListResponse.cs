using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Queenwood.Core.Client.Ebay.Model
{
    [XmlRoot(ElementName = "GetSellerListResponse", Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public class GetSellerListResponse
    {
        [XmlElement(ElementName = "ItemArray", Namespace = "urn:ebay:apis:eBLBaseComponents")]
        public ItemArray ItemArray { get; set; }
    }

    [XmlRoot(ElementName = "ItemArray", Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public class ItemArray
    {
        [XmlElement(ElementName = "Item", Namespace = "urn:ebay:apis:eBLBaseComponents")]
        public List<Item> Items { get; set; }

        public ItemArray()
        {
            Items = new List<Item>();
        }
    }

    [XmlRoot(ElementName = "Item", Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public class Item
    {
        [XmlElement(ElementName = "ListingDetails", Namespace = "urn:ebay:apis:eBLBaseComponents")]
        public ListingDetails ListingDetails { get; set; }
        [XmlElement(ElementName = "PrimaryCategory", Namespace = "urn:ebay:apis:eBLBaseComponents")]
        public PrimaryCategory PrimaryCategory { get; set; }
        [XmlElement(ElementName = "Quantity", Namespace = "urn:ebay:apis:eBLBaseComponents")]
        public string Quantity { get; set; }
        [XmlElement(ElementName = "SellingStatus", Namespace = "urn:ebay:apis:eBLBaseComponents")]
        public SellingStatus SellingStatus { get; set; }
        [XmlElement(ElementName = "Title", Namespace = "urn:ebay:apis:eBLBaseComponents")]
        public string Title { get; set; }
        [XmlElement(ElementName = "PictureDetails", Namespace = "urn:ebay:apis:eBLBaseComponents")]
        public PictureDetails PictureDetails { get; set; }

        public bool Hide { get; set; }
    }

    [XmlRoot(ElementName = "ListingDetails", Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public class ListingDetails
    {
        [XmlElement(ElementName = "ViewItemURL", Namespace = "urn:ebay:apis:eBLBaseComponents")]
        public string ViewItemURL { get; set; }
    }

    [XmlRoot(ElementName = "PrimaryCategory", Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public class PrimaryCategory
    {
        [XmlElement(ElementName = "CategoryID", Namespace = "urn:ebay:apis:eBLBaseComponents")]
        public string CategoryID { get; set; }
        [XmlElement(ElementName = "CategoryName", Namespace = "urn:ebay:apis:eBLBaseComponents")]
        public string CategoryName { get; set; }
    }

    //[XmlRoot(ElementName = "CurrentPrice", Namespace = "urn:ebay:apis:eBLBaseComponents")]
    //public class CurrentPrice
    //{
    //    [XmlAttribute(AttributeName = "currencyID")]
    //    public string CurrencyID { get; set; }
    //    [XmlText]
    //    public string Text { get; set; }
    //}

    [XmlRoot(ElementName = "SellingStatus", Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public class SellingStatus
    {
        [XmlElement(ElementName = "CurrentPrice", Namespace = "urn:ebay:apis:eBLBaseComponents")]
        public decimal? CurrentPrice { get; set; }
    }

    [XmlRoot(ElementName = "PictureDetails", Namespace = "urn:ebay:apis:eBLBaseComponents")]
    public class PictureDetails
    {
        [XmlElement(ElementName = "GalleryURL", Namespace = "urn:ebay:apis:eBLBaseComponents")]
        public string GalleryURL { get; set; }
        [XmlElement(ElementName = "PhotoDisplay", Namespace = "urn:ebay:apis:eBLBaseComponents")]
        public string PhotoDisplay { get; set; }
        [XmlElement(ElementName = "PictureURL", Namespace = "urn:ebay:apis:eBLBaseComponents")]
        public List<string> PictureURL { get; set; }
    }
}



