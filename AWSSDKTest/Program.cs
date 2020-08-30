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
            
            foreach(var bucket in buckets_response.Buckets)
            {
                ListObjectsV2Request req = new ListObjectsV2Request();
                req.BucketName = bucket.BucketName;
                ListObjectsV2Response res = await client.ListObjectsV2Async(req);
                int len = res.S3Objects.Count;
                Console.WriteLine(bucket.BucketName + " date:" + bucket.CreationDate + " has " + len + " objects.");
            }

            Console.WriteLine("Thanks for all the fish");
        }
    }
}
