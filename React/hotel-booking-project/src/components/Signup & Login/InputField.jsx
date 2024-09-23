import './InputField.css'

function InputField({ labelText, inputType, inputId, inputName, value, onChange }) {

    return (
        <div className="input-field">
            <input
                type={inputType}
                id={inputId}
                name={inputName}
                placeholder={labelText}
                value={value}
                onChange={onChange}
            />
        </div>
    )
}

export default InputField