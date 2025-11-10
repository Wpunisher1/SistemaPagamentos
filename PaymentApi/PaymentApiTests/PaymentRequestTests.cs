using PaymentApi.Requests;

public class PaymentRequestTests
{
    [Fact]
    public void PaymentRequest_Should_Set_And_Get_Properties()
    {
        // Arrange: cria uma instância da classe que será testada
        var request = new PaymentRequest();

        // Act: atribui valores às propriedades
        request.AccountId = "user001";   // aqui testamos se a propriedade AccountId aceita atribuição de string
        request.Amount = 150.75m;        // aqui testamos se a propriedade Amount aceita atribuição de decimal

        // Assert: verifica se os valores atribuídos foram armazenados corretamente
        Assert.Equal("user001", request.AccountId); // garante que o valor de AccountId foi persistido
        Assert.Equal(150.75m, request.Amount);      // garante que o valor de Amount foi persistido
    }

    [Fact]
    public void PaymentRequest_Default_Should_Have_Null_AccountId_And_Zero_Amount()
    {
        // Arrange: cria uma nova instância sem atribuir valores
        var request = new PaymentRequest();

        // Assert:
        Assert.Null(request.AccountId); // como usamos "default!" na propriedade, o valor inicial é null
        Assert.Equal(0m, request.Amount); // tipos numéricos (decimal) iniciam com 0 por padrão
    }
}