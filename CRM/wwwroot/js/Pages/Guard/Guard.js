$(document).ready(function () {
    $(document).on('change', '[id^=numberGroup-]', function () {
        var selectedValue = $(this).val();
        console.log("Dropdown1 selected value: " + selectedValue);

        var index = $(this).attr('id').split('-')[1];
        console.log("Index: " + index);

        $.ajax({
            url: '/_2D/GetAddressesByNumberGroup',
            type: 'GET',
            data: { numberGroup: selectedValue },
            success: function (data) {
                console.log("Data from GetAddressesByNumberGroup: ", data);

                var textarea = $('#address-' + index);
                textarea.val('');

                if (data.length === 0) {
                    console.log('No data returned.');
                    textarea.val('No address found');
                } else {
                    $.each(data, function (i, value) {
                        textarea.val(function (i, oldValue) {
                            return oldValue + value + '\n';
                        });
                    });
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("Error in AJAX request: ", textStatus, errorThrown);
                alert('Error loading address data: ' + textStatus);
            }
        });
        console.log(selectedValue)
        $.ajax({
            url: '/_2D/GetDataFromDBNumGroup',
            type: 'GET',
            data: { num: selectedValue },
            dataType: 'json',
            success: function (data) {
                console.log("Data from GetDataFromDBNumGroup: ", data);

                var textarea = $('#NameGroup-' + index);
                textarea.val('');

                if (data && data.length > 0) {
                    $.each(data, function (i, value) {
                        textarea.val(function (i, oldValue) {
                            return oldValue + value + '\n';
                        });
                    });
                } else {
                    console.log('No data returned.');
                    textarea.val('No data found');
                }

                adjustHeight(textarea[0]);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("Error in AJAX request: ", textStatus, errorThrown);
                alert('Error loading group data: ' + textStatus);
            }
        });
    });

    function showAlert(message, type) {
        const alertHTML = `
            <div class="alert alert-${type} alert-dismissible fade show" role="alert">
                ${message}
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </div>
        `;
        $('#alert-container').html(alertHTML);
        setTimeout(() => {
            $('.alert').alert('close');
        }, 5000);
    }
});

function adjustHeight(textarea) {
    textarea.style.height = 'auto';
    textarea.style.height = textarea.scrollHeight + 'px';
}

document.addEventListener('DOMContentLoaded', function () {
    var saveButton = document.getElementById('saveForm');

    saveButton.addEventListener('click', function (event) {
        var form = document.getElementById('dataForm');
        var isEmpty = false;

        for (let i = 0; i < form.elements.length; i++) {
            let element = form.elements[i];
            let elementName = element.name || "";

            if (
                elementName.includes(".NumberGroup") ||
                elementName.includes(".address") ||
                elementName.includes(".OhronnaComp") ||
                (elementName.includes(".NumDog") && !elementName.includes(".NumDog2")) ||
                (elementName.includes(".StrokDii") && !elementName.includes(".StrokDii2"))
            ) {
                if (!element.value.trim()) {
                    isEmpty = true;
                    element.classList.add("is-invalid");
                } else {
                    element.classList.remove("is-invalid");
                }
            }
        }

        if (isEmpty) {
            event.preventDefault();
        }
    });
});

document.addEventListener('DOMContentLoaded', function () {
    const textareas = document.querySelectorAll('textarea.auto-resize');

    textareas.forEach(textarea => {
        const adjustHeight = (textarea) => {
            textarea.style.height = 'auto';
            textarea.style.height = textarea.scrollHeight + 'px';
        };

        adjustHeight(textarea);

        textarea.addEventListener('input', () => adjustHeight(textarea));
    });
});
