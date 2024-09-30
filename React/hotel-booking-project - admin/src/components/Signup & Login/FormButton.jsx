import './FormButton.css';

function FormButton({ type, text }) {

    return (
        <button type={type} className="form-button">
            {text}
        </button>
    )
}

export default FormButton