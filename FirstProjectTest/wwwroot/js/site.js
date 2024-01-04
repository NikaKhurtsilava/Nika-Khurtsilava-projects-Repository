// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function updateBalance() {
    console.log("Updating balance...");

    
    $.ajax({
        url: '/Wallet/GetCurrentBalance',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            console.log("Balance update successful:", data);
            // Update the balance in the navbar
            $('#current-balance').text('' + data.currentBalance.toFixed(2));
        },
        error: function (error) {
            console.error('Error updating balance:', error);
            // Handle errors if needed
        }
    });
}

// Call the updateBalance function when the page loads
$(document).ready(function () {
    updateBalance();

    // Set up an interval to refresh the balance every 30 seconds
    setInterval(updateBalance, 30000); // 30,000 milliseconds = 30 seconds
});