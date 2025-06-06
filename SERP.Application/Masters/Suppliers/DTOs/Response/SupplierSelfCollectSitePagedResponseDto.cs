﻿namespace SERP.Application.Masters.Suppliers.DTOs.Response
{
    public class SupplierSelfCollectSitePagedResponseDto
    {
        public int id { get; set; }
        public int self_collect_site_id { get; set; }
        public string status_flag { get; set; }
        public DateTime created_on { get; set; }
        public string created_by { get; set; }
        public DateTime? last_modified_on { get; set; }
        public string? last_modified_by { get; set; }
    }
}
