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
            setAvailabilityMessage('Vælg venligst check-in og check-ud datoer.');
            return;
        }


        try {
            const token = localStorage.getItem('token'); // Get the token from local storage
            if (!token) {
                throw new Error('No token found');
            }

            const payload = JSON.parse(atob(token.split('.')[1])); // Decode JWT token to get payload
            const email = payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"]; // Extract email from token
            if (!email) {
                throw new Error('Email not found in token');
            }

            // Fetch user ID using the email
            const userResponse = await fetch(`https://localhost:7207/api/Users/email/${encodeURIComponent(email)}`, { // Fetch user ID using the email
                headers: {
                    'accept': 'application/json',
                    'Authorization': `Bearer ${token}`
                }
            });

            if (!userResponse.ok) { // Check if the response is not OK
                console.log(userResponse);
                throw new Error('Failed to fetch user ID');
            }

            const userData = await userResponse.json(); // Parse the response JSON
            const userId = userData.id; // Extract the user ID
            if (!userId) {
                throw new Error('User ID not found');
            }

            // Adjust dates for timezone offset
            const adjustForTimezone = (date) => {
                const offset = date.getTimezoneOffset();
                return new Date(date.getTime() - (offset * 60 * 1000)).toISOString();
            };

            // Create a booking object
            const booking = {
                id: 0,
                bookingDate: new Date().toISOString(),
                bookingStartDate: adjustForTimezone(checkInDate),
                bookingEndDate: adjustForTimezone(checkOutDate),
                roomId: 0,
                userId: userId,
                totalPrice: 0,
                roomType: roomType
            };

            console.log('Booking Object:', booking); // Log the booking object

            // Send a POST request to create a booking
            const response = await fetch('https://localhost:7207/api/Booking', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json', // Set the content type to JSON
                    'Authorization': `Bearer ${token}` // Set the authorization header
                },
                body: JSON.stringify(booking)
            });

            if (response.ok) {
                setAvailabilityMessage('Booking gennemført.');

                // Send a confirmation email
                const emailResponse = await fetch('https://localhost:7207/Mail', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        emailToId: email, // Send the email to the user
                        emailToName: email, // Set the name to the email
                        emailSubject: `Booking Confirmation`,
                        emailBody: `Dear customer,\n\nThank you for booking with us! Here are your booking details:\n\nCheck-in Date: ${new Date(checkInDate).toLocaleDateString()}\nCheck-out Date: ${new Date(checkOutDate).toLocaleDateString()}\n\nWe look forward to your stay!\n\nBest regards,\nHotel None Inclusive`
                    })
                });

                if (emailResponse.ok) {
                    console.log('Bekræftelsesmail sendt succesfuldt.');
                } else {
                    console.error('Kunne ikke sende bekræftelsesmail.');
                }

            } else {
                const errorData = await response.json();
                setAvailabilityMessage(errorData.message || 'Booking mislykkedes.');
            }
        } catch (error) {
            console.error('Error:', error);
            setAvailabilityMessage('Der opstod en fejl under booking af værelset.');
        } finally {
            setOpen(false);
        }
    };

    // Function to check room availability
    const checkRoomAvailability = async () => {
        if (!checkInDate || !checkOutDate) {
            setAvailabilityMessage('Vælg venligst både indtjeknings- og udtjekningsdatoer.');
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
                    setAvailabilityMessage('Rummet er ledigt.');
                    setTotalPrice(data.totalPrice);
                } else {
                    setAvailabilityMessage(data.message || 'Rummet er desværre ikke ledigt på de valgte datoer.');
                    setTotalPrice(null);
                }
            } else {
                const text = await response.text();
                setAvailabilityMessage(text || 'Rummet er desværre ikke ledigt på de valgte datoer.');
                setTotalPrice(null);
            }
        } catch (error) {
            console.error('Error:', error);
            setAvailabilityMessage('Der opstod en fejl under tjek af ledighed.');
        }
    };

    const today = new Date();

    // CARD COMPONENT
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
                    {roomType} Rum
                </Typography>
                <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                    {description}
                </Typography>
                <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                    Etage: {floor}
                </Typography>
                <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                    Pris: DKK {price}
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
                        Vælg Check-in og Check-ud Datoer
                    </Typography>
                    <DatePicker
                        selected={checkInDate}
                        onChange={(date) => setCheckInDate(date)}
                        selectsStart
                        startDate={checkInDate}
                        endDate={checkOutDate}
                        minDate={today}
                        placeholderText="Check-in Dato"
                        inline
                    />
                    <DatePicker
                        selected={checkOutDate}
                        onChange={(date) => setCheckOutDate(date)}
                        selectsEnd
                        startDate={checkInDate}
                        endDate={checkOutDate}
                        minDate={checkInDate || today}
                        placeholderText="Check-ud Dato"
                        inline
                    />
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', width: '100%' }}>
                        <Button onClick={handleClose} color="secondary">Annuller</Button>
                        <Button onClick={handleConfirm} color="primary">Bekræft</Button>
                    </Box>
                    <Button onClick={checkRoomAvailability} color="primary">Tjek om værelset er ledigt</Button>
                    {availabilityMessage && <Typography variant="body2">{availabilityMessage}</Typography>}
                    {totalPrice !== null && <Typography variant="body2">Samlet Pris: DKK{totalPrice}</Typography>}
                </Box>
            </Modal>
        </Card>
    );
}
