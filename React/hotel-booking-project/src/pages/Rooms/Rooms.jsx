import React, { useEffect, useState } from 'react';
import MultiActionAreaCard from './Card';
import './Rooms.css';

function Rooms() {
    const [rooms, setRooms] = useState([]);

    useEffect(() => {
        fetch('https://localhost:7207/api/Rooms')
            .then(response => response.json())
            .then(data => setRooms(data))
            .catch(error => console.error('Error fetching rooms:', error));
    }, []);

    return (
        <div>

            <div className="rooms-container">
                {rooms.map(room => (
                    <MultiActionAreaCard
                        key={room.id}
                        imageURL={room.imageURL}
                        price={room.price}
                        roomType={room.roomType}
                        roomNumber={room.roomNumber}
                        description={room.description}
                        floor={room.floor}
                    />
                ))}
            </div>
        </div>
    );
}

export default Rooms;