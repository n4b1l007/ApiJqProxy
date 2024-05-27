$(function () {
    var paymentAPI = apijqproxy.payment;

    var allPayments = paymentAPI.payment_Get("");
    allPayments.then(data => console.log(data))
        .catch(error => console.error(error));

    $("#save_payment").click(function () {
        paymentAPI.payment_Post({
            "Date": "2024-05-23T15:30:00",
            "TemperatureC": 25,
            "TemperatureF": 77,
            "Summary": "Partly cloudy"
        },null,null)
            .then(data => console.log(data))
            .catch(error => console.error(error));
    });
    $("#delete_payment").click(function () {
        //paymentAPI.payment_Delete
    });
})