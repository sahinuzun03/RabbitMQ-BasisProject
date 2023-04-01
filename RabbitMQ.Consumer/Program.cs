using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


//Bağlantı oluşturuldu
ConnectionFactory factory = new();
factory.Uri = new("amqps://agynxcdk:czCiBq-QnX_MgyDat8Iqxl2bVtBOGZFH@woodpecker.rmq.cloudamqp.com/agynxcdk");

//Bağlantı Aktifleştirme ve Kanal Açma

using IConnection connection = factory.CreateConnection(); //IDisposable olduğu için using koydum ki kendisini patlatması için.
using IModel channel = connection.CreateModel(); //IDisposable olduğu için using koydum ki kendisini patlatması için.

//Queue Oluşturm
channel.QueueDeclare(queue: "example-queue", exclusive: false);
//Consumer tarafında da kuyruk aynı yapı ile tanımlanmalıdır.

EventingBasicConsumer consumer = new(channel);
//Mesajı okumaya dnlemeye başlıyoruz
//AutoAck mesaj silinecek mi vs
//consumer en sondaki mesajı tüketecek olan kişi
channel.BasicConsume(queue: "example-queue", false, consumer);//Mesajı tüketecek 

consumer.Received += (sender, e) =>
{
    //Kuyruğa gelen mesajın işlendiği yerdir.
    //e.Body => Kuyruktaki mesajı bütünsel olarak bize getirecektir
    //e.Body.Span veya e.Body.ToArray() -> Kuyruktak mesajın byte verisini getirecektir

    var message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};

Console.Read();