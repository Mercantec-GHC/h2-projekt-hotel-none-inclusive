import React, { useEffect, useState } from 'react';
import MultiActionAreaCard from './Card';
import './Rooms.css';

const AllRooms = () => {
    const [rooms, setRooms] = useState([]);

    useEffect(() => {
        fetch('https://localhost:7207/api/Rooms')
            .then(response => response.json())
            .then(data => {
                setRooms(data); // Set all rooms data
            })
            .catch(error => console.error('Error fetching rooms:', error));
    }, []); // Empty array as second argument to only run the effect once

    return (
        <div className="rooms-container">
            {rooms.map((room) => (
                <MultiActionAreaCard
                    key={room.id}
                    roomType={room.roomType}
                    roomNumber={room.roomNumber}
                    price={room.price}
                    floor={room.floor}
                    description={room.description}
                    imageURL={room.imageURL}
                />
            ))}
        </div>
    );
};

export default AllRooms;