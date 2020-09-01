using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.Polly;
using Amazon.Polly.Model;
using System.Media;
using System.IO;
using System.Security.Cryptography;

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
                // NOTE if this finds a folder it includes the folder as an object, and its content as
                // more objects.  I thought folders didn't exist as such, but it seems they do.
                ListObjectsV2Response res = await client.ListObjectsV2Async(req);
                int len = res.S3Objects.Count;
                Console.WriteLine(bucket.BucketName + " date:" + bucket.CreationDate + " has " + len + " objects.");
                if (len > 0)
                {
                    foreach(var obj in res.S3Objects)
                    {
                        Console.WriteLine("   " + obj.Key + " " + obj.Size + " " + obj.StorageClass + " " + obj.ETag);
                    }
                }
            }

            var pollyclient = new AmazonPollyClient();
            SynthesizeSpeechRequest request = new SynthesizeSpeechRequest();
            request.Text = "Hello my lovely little world.";
            request.OutputFormat = OutputFormat.Mp3;
            request.VoiceId = VoiceId.Emma;
            var pollyres = await pollyclient.SynthesizeSpeechAsync(request);
            var fs = new FileStream("c:/tmp/hw.mp3", FileMode.Create);
            var instr = pollyres.AudioStream;

            byte[] bytes = new byte[256];
            int count = 0;
            while (true) //count < instr.Length)
            {
                int c = instr.Read(bytes, 0, 256);
                if (c == 0) break;
                fs.Write(bytes,0,c);
                count += c;
            }
            fs.Close();
            var okater = new NetCoreAudio.Player();

            await okater.Play("c:/tmp/hw.mp3");
            Console.WriteLine("Thanks for all the fish");
        }
    }
}
