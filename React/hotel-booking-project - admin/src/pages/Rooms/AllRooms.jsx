import React, { useEffect, useState } from 'react';
import CardAllRooms from './CardAllRooms.jsx';
import './Rooms.css';
import { TextField } from '@mui/material';

const AllRooms = () => {
    const [rooms, setRooms] = useState([]);
    const [searchQuery, setSearchQuery] = useState('');

    useEffect(() => {
        fetch('https://localhost:7207/api/Rooms')
            .then(response => response.json())
            .then(data => {
                setRooms(data); // Set all rooms data
            })
            .catch(error => console.error('Error fetching rooms:', error));
    }, []); // Empty array as second argument to only run the effect once

const handleDeleteRoom = async (roomId) => {
    try {
        await fetch(`https://localhost:7207/api/Rooms/${roomId}`, {
            method: 'DELETE',
        });
        setRooms(rooms.filter(room => room.id !== roomId));
    } catch (error) {
        console.error('Error deleting room:', error);
    }
};

    const filteredRooms = rooms.filter(room =>
        room.roomType.toLowerCase().includes(searchQuery.toLowerCase())
    );

    return (
        <div className="all-rooms-container">
            <div style={{ display: 'flex', justifyContent: 'center', width: '100%', marginBottom: '20px' }}>
                <TextField
                    label="Søg efter værelses type"
                    variant="outlined"
                    margin="normal"
                    value={searchQuery}
                    onChange={(e) => setSearchQuery(e.target.value)}
                    sx={{ width: '300px' }} // Adjust the width as needed
                />
            </div>
            {filteredRooms.map((room) => (
                <CardAllRooms
                    key={room.id}
                    roomId={room.id}
                    roomType={room.roomType}
                    roomNumber={room.roomNumber}
                    price={room.price}
                    floor={room.floor}
                    description={room.description}
                    imageURL={room.imageURL}
                    onDelete={() => handleDeleteRoom(room.id)}
                />
            ))}
        </div>
    );
};

export default AllRooms;