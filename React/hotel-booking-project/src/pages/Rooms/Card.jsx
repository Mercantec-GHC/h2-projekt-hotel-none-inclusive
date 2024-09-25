import * as React from 'react';
import { useState } from 'react';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import CardMedia from '@mui/material/CardMedia';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import CardActions from '@mui/material/CardActions';
import Modal from '@mui/material/Modal';
import Box from '@mui/material/Box';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';

export default function MultiActionAreaCard({ imageURL, price, roomType, description, floor }) {
    const [open, setOpen] = useState(false);
    const [checkInDate, setCheckInDate] = useState(null);
    const [checkOutDate, setCheckOutDate] = useState(null);
    const [availabilityMessage, setAvailabilityMessage] = useState('');
    const [totalPrice, setTotalPrice] = useState(null);

    const handleClickOpen = () => setOpen(true);
    const handleClose = () => setOpen(false);
    const handleConfirm = async () => {
        if (!checkInDate || !checkOutDate) {
            setAvailabilityMessage('Please select both check-in and check-out dates.');
            return;
        }

        try {
            const token = localStorage.getItem('token');
            if (!token) {
                throw new Error('No token found');
            }

            const payload = JSON.parse(atob(token.split('.')[1])); // Decode JWT token to get payload
            const email = payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];
            if (!email) {
                throw new Error('Email not found in token');
            }

            // Fetch user ID using the email
            const userResponse = await fetch(`https://localhost:7207/api/Users/email/${encodeURIComponent(email)}`, {
                headers: {
                    'accept': 'application/json',
                    'Authorization': `Bearer ${token}`
                }
            });

            if (!userResponse.ok) {
                console.log(userResponse);
                throw new Error('Failed to fetch user ID');
            }

            const userData = await userResponse.json();
            const userId = userData.id;
            if (!userId) {
                throw new Error('User ID not found');
            }

            const booking = {
                id: 0,
                bookingDate: new Date().toISOString(),
                bookingStartDate: checkInDate.toISOString(),
                bookingEndDate: checkOutDate.toISOString(),
                roomId: 0,
                userId: userId,
                totalPrice: totalPrice,
                roomType: roomType
            };

            console.log('Booking Object:', booking); // Log the booking object

            const response = await fetch('https://localhost:7207/api/Booking', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(booking)
            });

            if (response.ok) {
                setAvailabilityMessage('Booking successful.');
            } else {
                const errorData = await response.json();
                setAvailabilityMessage(errorData.message || 'Booking failed.');
            }
        } catch (error) {
            console.error('Error:', error);
            setAvailabilityMessage('An error occurred while booking the room.');
        } finally {
            setOpen(false);
        }
    };

    const checkRoomAvailability = async () => {
        if (!checkInDate || !checkOutDate) {
            setAvailabilityMessage('Please select both check-in and check-out dates.');
            return;
        }

        try {
            const response = await fetch(
                `https://localhost:7207/api/Rooms/CheckRoomAvailability?roomType=${roomType}&startDate=${checkInDate.toISOString()}&endDate=${checkOutDate.toISOString()}`
            );
            // Check if the response is JSON
            const contentType = response.headers.get('content-type');
            if (contentType && contentType.indexOf('application/json') !== -1) {
                const data = await response.json();
                if (response.ok) {
                    setAvailabilityMessage('Room is available.');
                    setTotalPrice(data.totalPrice);
                } else {
                    setAvailabilityMessage(data.message || 'Room is not available for the selected dates.');
                    setTotalPrice(null);
                }
            } else {
                const text = await response.text();
                setAvailabilityMessage(text || 'Room is not available for the selected dates.');
                setTotalPrice(null);
            }
        } catch (error) {
            console.error('Error:', error);
            setAvailabilityMessage('An error occurred while checking availability.');
        }
    };

    const today = new Date();

    return (
        <Card sx={{ maxWidth: 345 }}>
            <CardMedia
                component="img"
                height="140"
                image={imageURL}
                alt={roomType}
            />
            <CardContent>
                <Typography gutterBottom variant="h5" component="div">
                    {roomType} Room
                </Typography>
                <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                    {description}
                </Typography>
                <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                    Floor: {floor}
                </Typography>
                <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                    Price: ${price}
                </Typography>
            </CardContent>
            <CardActions>
                <Button size="small" color="primary" onClick={handleClickOpen}>
                    Book
                </Button>
            </CardActions>
            <Modal
                open={open}
                onClose={handleClose}
                aria-labelledby="modal-title"
                aria-describedby="modal-description"
            >
                <Box
                    sx={{
                        position: 'absolute',
                        top: '50%',
                        left: '50%',
                        transform: 'translate(-50%, -50%)',
                        width: 400,
                        bgcolor: 'background.paper',
                        border: '2px solid #000',
                        boxShadow: 24,
                        p: 4,
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                        gap: 2,
                    }}
                >
                    <Typography id="modal-title" variant="h6" component="h2">
                        Select Check-in and Check-out Dates
                    </Typography>
                    <DatePicker
                        selected={checkInDate}
                        onChange={(date) => setCheckInDate(date)}
                        selectsStart
                        startDate={checkInDate}
                        endDate={checkOutDate}
                        minDate={today}
                        placeholderText="Check-in Date"
                        inline
                    />
                    <DatePicker
                        selected={checkOutDate}
                        onChange={(date) => setCheckOutDate(date)}
                        selectsEnd
                        startDate={checkInDate}
                        endDate={checkOutDate}
                        minDate={checkInDate || today}
                        placeholderText="Check-out Date"
                        inline
                    />
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', width: '100%' }}>
                        <Button onClick={handleClose} color="secondary">Cancel</Button>
                        <Button onClick={handleConfirm} color="primary">Confirm</Button>
                    </Box>
                    <Button onClick={checkRoomAvailability} color="primary">Check Room Availability</Button>
                    {availabilityMessage && <Typography variant="body2">{availabilityMessage}</Typography>}
                    {totalPrice !== null && <Typography variant="body2">Total Price: ${totalPrice}</Typography>}
                </Box>
            </Modal>
        </Card>
    );
}
