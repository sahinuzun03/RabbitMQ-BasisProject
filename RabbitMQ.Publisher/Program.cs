using RabbitMQ.Client;
using System.Text;


//Bağlantı oluşturuldu
ConnectionFactory factory = new();
factory.Uri = new("amqps://agynxcdk:czCiBq-QnX_MgyDat8Iqxl2bVtBOGZFH@woodpecker.rmq.cloudamqp.com/agynxcdk");

//Bağlantı Aktifleştirme ve Kanal Açma

using IConnection connection = factory.CreateConnection(); //IDisposable olduğu için using koydum ki kendisini patlatması için.
using IModel channel = connection.CreateModel(); //IDisposable olduğu için using koydum ki kendisini patlatması için.

//Queue Oluşturm
channel.QueueDeclare(queue: "example-queue", exclusive: false); //Exclusive mesajların erişilebilmesi için false olarak ayarlanması lazım.Birden fazla channel tarafından bu kodun kullanılabileceğin söyledik.

//Queue mesaj gönderme

//RabbitMQ kuyruğa atacağı mesajları byte türünden kabul etmektedir.Bu yüzden mesajları byte türüne dönüştürmemiz gerekecektir.


for (int i = 0; i < 100; i++)
{
    byte[] message = Encoding.UTF8.GetBytes("Merhaba" + i);

    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message); //Defaut exchange : directexchange oldu // routingkey olarak kuyruk adını verdik istediğimizi verebiliriz ve message'ı gödneridk
}


Console.Read();


