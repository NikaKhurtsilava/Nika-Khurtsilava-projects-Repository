    // deposit.js
    $(document).ready(function () {
        $("#depositButton").unbind("click").click(function () {
            console.log("starting ajax call for deposit");
            event.preventDefault();
            var amount = $("#amount").val();

            // Make AJAX request
            $.ajax({
                url: "/Deposit/MakeDeposit",
                type: "POST",
                data: { amount: amount },
                success: function (result) {
                    if (result.success) {
                        console.log(result.message);
                        alert(result.message);
                    } else {
                        console.error(result.message);
                        alert("Error: " + result.message);
                    }
                },
                error: function (error) {
                    console.error("Deposit failed", error);
                    alert("An error occurred while processing your request.");
                }
            });
        });
    });
