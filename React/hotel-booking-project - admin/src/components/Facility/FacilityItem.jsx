import './FacilityItem.css'

function FacilityItem({ key, icon, facility, nameForText, nameForClass }) {
    return (
        <div key={key} className={nameForClass}>
            {icon}
            <p className={nameForText}>{facility}</p>
        </div>
    )
}

export default FacilityItem
