import "./CreateRoomPage.css";
import * as React from 'react';
import Box from '@mui/material/Box';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';

function CreateRoomPage() {
    const [roomType, setRoomType] = React.useState('');
    const [roomNumber, setRoomNumber] = React.useState('');
    const [pricePerNight, setPricePerNight] = React.useState('');
    const [floor, setFloor] = React.useState('');
    const [description, setDescription] = React.useState('');
    const [imageLink, setImageLink] = React.useState('');
    const [successMessage, setSuccessMessage] = React.useState('');

    const handleChange = (event) => {
        setRoomType(event.target.value);
    };

    const handleButtonClick = async () => {
        const roomDetails = {
            roomNumber,
            roomType,
            pricePerNight,
            floor,
            description,
            imageURL: imageLink
        };

        try {
            const response = await fetch('https://localhost:7207/api/Rooms', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'accept': 'text/plain'
                },
                body: JSON.stringify(roomDetails)
            });

            if (response.ok) {
                const data = await response.json();
                console.log('Room created successfully:', data);
                setSuccessMessage('Værelset blev oprettet');
                // Clear the input fields
                setRoomType('');
                setRoomNumber('');
                setPricePerNight('');
                setFloor('');
                setDescription('');
                setImageLink('');
            } else {
                console.error('Failed to create room:', response.statusText);
            }
        } catch (error) {
            console.error('Error:', error);
        }
    };

    return (
        <div className="create-room-page-container">
            <h1>Opret Værelse</h1>
            <div className="create-room-flex">
                <Box className="create-room-box" sx={{ maxWidth: '40ch' }}>
                    <FormControl fullWidth sx={{ marginRight: '0.5rem' }}>
                        <InputLabel id="create-room-dropdown-label">Type</InputLabel>
                        <Select
                            labelId="create-room-dropdown-label"
                            id="create-room-dropdown-select"
                            value={roomType}
                            label="Type"
                            onChange={handleChange}
                        >
                            <MenuItem value="Standard">Standard</MenuItem>
                            <MenuItem value="Deluxe">Deluxe</MenuItem>
                            <MenuItem value="Premium">Premium</MenuItem>
                        </Select>
                    </FormControl>
                    <TextField
                        id="outlined-room-number"
                        label="Værelsesnummer"
                        variant="outlined"
                        value={roomNumber}
                        onChange={(e) => setRoomNumber(e.target.value)}
                        sx={{ marginLeft: '0.5rem' }}
                    />
                </Box>

                <Box className="create-room-box" sx={{ maxWidth: '40ch' }}>
                    <TextField
                        id="outlined-price-per-night"
                        label="Pris Pr Nat"
                        variant="outlined"
                        value={pricePerNight}
                        onChange={(e) => setPricePerNight(e.target.value)}
                        sx={{ marginRight: '0.5rem' }}
                    />
                    <TextField
                        id="outlined-floor"
                        label="Etage"
                        variant="outlined"
                        value={floor}
                        onChange={(e) => setFloor(e.target.value)}
                        sx={{ marginLeft: '0.5rem' }}
                    />
                </Box>

                <Box className="create-room-box" sx={{ maxWidth: '40ch' }}>
                    <TextField
                        id="outlined-description"
                        label="Beskrivelse"
                        variant="outlined"
                        value={description}
                        onChange={(e) => setDescription(e.target.value)}
                        sx={{ marginRight: '0.5rem' }}
                    />
                    <TextField
                        id="outlined-image-link"
                        label="Billede (link)"
                        variant="outlined"
                        value={imageLink}
                        onChange={(e) => setImageLink(e.target.value)}
                        sx={{ marginLeft: '0.5rem' }}
                    />
                </Box>
            </div>
            <br />
            <Button variant="contained" onClick={handleButtonClick}>Opret værelse</Button>
            {successMessage && <Typography variant="body2" color="success">{successMessage}</Typography>}
        </div>
    )
}

export default CreateRoomPage;
