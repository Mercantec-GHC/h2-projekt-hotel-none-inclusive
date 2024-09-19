import './TestimonialItem.css';

function TestimonialItem({ key, name, country, text, nameForClass, nameForText }) {
    return (
        <div key={key} className={nameForClass}>
            <p><span>&quot;</span> {text} <span>&quot;</span></p>
            <p className={nameForText}>{name}, {country}</p>
        </div>
    )
}

export default TestimonialItem