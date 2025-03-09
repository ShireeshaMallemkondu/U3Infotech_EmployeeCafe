
import React from 'react';

const TextBox = ({ label,name, value, onChange, minLength, maxLength, required }) => {
    return (
        <div>
            <label>{label}</label>
            <input
                type="text"
                name={name}
                value={value}
                onChange={onChange}
                minLength={minLength}
                maxLength={maxLength}
                required={required}
            />
        </div>
    );
};

export default TextBox;
