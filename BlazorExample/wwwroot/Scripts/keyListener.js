window.keyPressFunctions = {
    listenForShift: function (dotNetReference) {
        function shiftKeyDown(event) {
            if (event.key === "Shift") {
                dotNetReference.invokeMethodAsync('OnShiftKeyPress', true);
            }
        }

        function shiftKeyUp(event) {
            if (event.key === "Shift") {
                dotNetReference.invokeMethodAsync('OnShiftKeyPress', false);
            }
        }

        document.addEventListener('keydown', shiftKeyDown);
        document.addEventListener('keyup', shiftKeyUp);

        return {
            dispose: function () {
                document.removeEventListener('keydown', shiftKeyDown);
                document.removeEventListener('keyup', shiftKeyUp);
            }
        };
    }
};