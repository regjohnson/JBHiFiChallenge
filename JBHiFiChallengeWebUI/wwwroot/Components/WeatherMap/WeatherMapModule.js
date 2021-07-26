WeatherMapModule = (function () {

    var init = function () {
        $('#btnSearch').on('click', function () {

            var city = $('#txtCity').val();
            var country = $('#txtCountry').val();
            var keyName = $('#txtKeyName').val();
            retrieve(city, country, keyName);
        });
    }

    var retrieve = function (city, country, keyName) {
        if (keyName == '') {
            keyName = 'Key1';
        }

        var dest = 'https://localhost:44374/' + 'WeatherMap';

        dest += '?cityName=' + encodeURIComponent(city) + '&countryName=' + encodeURIComponent(country);

        $('#btnSearch').attr('disabled', true);
        $('#searchResult').empty();

        $.ajax({
            url: dest,
            headers: {
                "accept": "application/json",
                "Access-Control-Allow-Origin": "*",
                "keyName": keyName
            },
            contentType: 'application/json, charset=utf-8',
            dataType: "json",
            type: 'GET',
            async: true,
            success: function (result, status, jqXHR) {
                var data = result.data;
                if (status == 'success') {
                    $('#searchResult').html(data);
                }
            },
            error: function (result, status, jqXHR) {
                var responseText = result.responseText;
                $('#searchResult').html(responseText);
            },
            complete: function () {
                $('#btnSearch').attr('disabled', false);
            }
        });
    }

    return {
        Init: init
    }
})();