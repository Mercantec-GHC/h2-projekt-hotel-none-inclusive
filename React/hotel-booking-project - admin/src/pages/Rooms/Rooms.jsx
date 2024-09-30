import React, { useEffect, useState } from 'react';
import MultiActionAreaCard from './Card';
import './Rooms.css';

function Rooms() {
    const [rooms, setRooms] = useState([]);

    useEffect(() => {
        fetch('https://localhost:7207/api/Rooms')
            .then(response => response.json())
            .then(data => {
                const uniqueRooms = [];
                const roomTypes = new Set(); // Set to store unique room types

                data.forEach(room => {
                    if (!roomTypes.has(room.roomType)) { // Check if the room type is unique
                        roomTypes.add(room.roomType); // Add the room type to the set
                        uniqueRooms.push(room); // Add the room to the uniqueRooms array
                    }
                });

                setRooms(uniqueRooms);
            })
            .catch(error => console.error('Error fetching rooms:', error));
    }, []);

    return (
        <div className="rooms-container">
            {rooms.map((room) => (
                <MultiActionAreaCard
                    key={room.id}
                    roomType={room.roomType}
                    price={room.price}
                    floor={room.floor}
                    description={room.description}
                    imageURL={room.imageURL}
                />
            ))}
        </div>
    );
}

export default Rooms;