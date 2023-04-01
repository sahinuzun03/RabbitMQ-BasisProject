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
channel.QueueDeclare(queue: "example-queue", exclusive: false,durable:true);
//Consumer tarafında da kuyruk aynı yapı ile tanımlanmalıdır.

EventingBasicConsumer consumer = new(channel);
//Mesajı okumaya dnlemeye başlıyoruz
//AutoAck mesaj silinecek mi vs
//consumer en sondaki mesajı tüketecek olan kişi
channel.BasicConsume(queue: "example-queue", autoAck:false, consumer);//Mesajı tüketecek 
channel.BasicQos(0, 1, false); // 0 olan prefetchSize : en büyük byte boyutlu mesajı belirtir 0 sınırsız demek // 1 olan prefetchcount aynı anda işleme alınacak olam mesaj sayısı // false değeri ise bu tüm consumerlar için mi yoksa sadece cçağrı yapanlar için mi geçerli onu belirtir.

consumer.Received += (sender, e) =>
{
    //Kuyruğa gelen mesajın işlendiği yerdir.
    //e.Body => Kuyruktaki mesajı bütünsel olarak bize getirecektir
    //e.Body.Span veya e.Body.ToArray() -> Kuyruktak mesajın byte verisini getirecektir

    var message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);

    channel.BasicAck(e.DeliveryTag, multiple: false);

    //BasicAck burada mesajın başarıyla işlenip işlenmediğini söylemesini sağlayacak

    //multiple birden fazla mesaja dait onay bildirisi gönderir false verilirsa sadece Dbu mesaja onay bilgisi verir
};

Console.Read();

//Round - Robin Dispatching : mesajların sıralı bir şekilde consumerlara gönderilmesi olayını ıfade etmektedir.

//Message Acknowledgement --> autoAck bunu ayarlıyor. Biz bunu false yaptığımız zaman autoack bunu acknowledgement aktif olmuş oluyor. 

//BasicNack ile işlenmeyen mesajlar geri gönderilebilirler. RabbitMQ'ya bilgi verip meajı tekrardan işletebiliriz. requeue : true olursa mesaj kuyruğa tekrardan işlenmek üzere eklecek false verilir ise kuyruğa eklenmeyecek ve silinecektir.

//BasicCancel ile kuyruktaki bütün mesajların işlenmesinin reddebiliriz.

//BasitReject ile istediğimiz mesajın işlenmesini reddedebilriiz.Burada da requeue parametresi bulumaktadır.

//BasicQos metodu ile mesaj işleme hızını ve teslimat sınrasını belirleyebilriiz.
