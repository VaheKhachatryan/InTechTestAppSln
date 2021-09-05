namespace Common.Caching
{
	/// <summary>
	/// Represents distributed cache configuration parameters
	/// </summary>
	public class DistributedCacheConfig
	{
		/// <summary>
		/// Gets the default cache time in Seconds
		/// </summary>
		public static int CacheTime => 900;

		/// <summary>
		/// Gets or sets connection string. Used when distributed cache is enabled
		/// </summary>
		public string ConnectionString { get; set; } = "127.0.0.1:6379,ssl=False";

		/// <summary>
		/// Gets or sets schema name. Used when distributed cache is enabled and DistributedCacheType property is set as SqlServer
		/// </summary>
		public string SchemaName { get; set; } = "dbo";

		/// <summary>
		/// Gets or sets table name. Used when distributed cache is enabled and DistributedCacheType property is set as SqlServer
		/// </summary>
		public string TableName { get; set; } = "DistributedCache";
		public string InstanceName { get; set; }
	}
}