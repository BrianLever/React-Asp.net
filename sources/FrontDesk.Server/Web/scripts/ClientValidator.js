//------------------------------------
// Phone number client validator class
//------------------------------------
function PhoneValidator(phoneParts, isRequired) {
    this._phoneParts = phoneParts;
    this._isRequired = isRequired;
}

PhoneValidator.prototype = {

    Validate: function() {



        //check is control empty if phone number is required
        var isEmpty = true;
        var hasInvalidFormat = false;

        for (var i = 0; i < this._phoneParts.length; i++) {
            var part = this._phoneParts[i];

            if (!part.IsEmpty() && isEmpty) {
                isEmpty = false;
            }

            if (!hasInvalidFormat && !part.IsValid()) {
                hasInvalidFormat = true;
            }
        }

        if (this._isRequired && isEmpty) {
            return false;
        } else if (!this._isRequired && isEmpty) {
            return true;
        } else if (hasInvalidFormat) {
            return false;
        }

        return true;
    }
}


//--------------------------------------------------------
// Phone part definition
//--------------------------------------------------------
function PhonePart(conotrlClientID, requiredLength) {
    this._control = document.getElementById(conotrlClientID);
    this._requiredLength = requiredLength;
}

PhonePart.prototype = {

    IsValid: function() {
        return this._control.value.length == this._requiredLength;
    },

    IsEmpty: function() {
        return this._control.value.length == 0;
    }
}
