using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace AWSSDKTest
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var client = new AmazonS3Client(RegionEndpoint.EUWest2);
            ListBucketsResponse buckets_response = await client.ListBucketsAsync();
            
            foreach(var buckets in buckets_response.Buckets)
            {
                Console.WriteLine(buckets.BucketName + " date:" + buckets.CreationDate);
            }

        }
    }
}
