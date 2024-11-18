$(document).ready(function () {

    $('#dropdown1').change(function () {
        var selectedValue = $(this).val();
        console.log("Dropdown1 selected value: " + selectedValue);
        $.ajax({
            url: '/_2D/GetDataFromDB',
            type: 'GET',
            data: { pokaznuk: selectedValue },
            success: function (data) {
                console.log("Data from GetDataFromDB: ", data);
                var dropdown2 = $('#dropdown2');
                dropdown2.empty();
                dropdown2.append('<option value="">Выберите значение в dropdown1 сначала</option>');
                $.each(data, function (index, value) {
                    dropdown2.append($('<option></option>').val(value).text(value));
                });
            },
            error: function () {
                alert('Error loading second dropdown data');
            }
        });
    });

    $('#dropdown2').change(function () {
        var selectedValue = $(this).val();
        console.log("Dropdown2 selected value: " + selectedValue);
        $.ajax({
            url: '/_2D/GetAdditionalData',
            type: 'GET',
            data: { pokaznuk: selectedValue },
            success: function (data) {
                console.log("Data from GetAdditionalData: ", data);
                var dropdown3 = $('#dropdown3');
                dropdown3.empty();
                dropdown3.append('<option value="">Выберите значение в dropdown2 сначала</option>');
                $.each(data, function (index, value) {
                    dropdown3.append($('<option></option>').val(value).text(value));
                });
            },
            error: function () {
                alert('Error loading third dropdown data');
            }
        });
    });
});