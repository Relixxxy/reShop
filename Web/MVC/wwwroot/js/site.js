(() => {
    const numberInput = document.getElementById('amount-input');

    numberInput.addEventListener('input', () => {
        const value = parseInt(numberInput.value);

        if (value < parseInt(numberInput.min)) {
            numberInput.value = numberInput.min;
        } else if (value > parseInt(numberInput.max)) {
            numberInput.value = numberInput.max;
        }
    });
})()