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
import TextField from '@mui/material/TextField';

export default function MultiActionAreaCard({ imageURL, price, roomType, description, floor, roomNumber }) {
    const [open, setOpen] = useState(false);
    const [checkInDate, setCheckInDate] = useState(null);
    const [checkOutDate, setCheckOutDate] = useState(null);
    const [availabilityMessage, setAvailabilityMessage] = useState('');
    const [totalPrice, setTotalPrice] = useState(null);
    const [email, setEmail] = useState('');

    const handleClickOpen = () => setOpen(true);
    const handleClose = () => setOpen(false);

    const generateRandomPassword = () => {
        const chars = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
        let password = '';
        for (let i = 0; i < 8; i++) {
            password += chars.charAt(Math.floor(Math.random() * chars.length));
        }
        return password;
    };

    const handleConfirm = async () => {
        if (!checkInDate || !checkOutDate) {
            setAvailabilityMessage('Please select both check-in and check-out dates.');
            return;
        }

        if (!email) {
            setAvailabilityMessage('Please enter an email.');
            return;
        }

        try {
            let userId;
            let newPassword = null;

            // Check if the email is already in the database
            const checkEmailResponse = await fetch(`https://localhost:7207/api/Users/email/${encodeURIComponent(email)}`, {
                method: 'GET',
                headers: {
                    'accept': 'text/plain'
                }
            });

            if (checkEmailResponse.ok) {
                const userData = await checkEmailResponse.json();
                userId = userData.id;
            } else if (checkEmailResponse.status === 404) {
                // If the email is not found, register a new user
                newPassword = generateRandomPassword();
                const registerResponse = await fetch('https://localhost:7207/api/Auth/register', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'accept': 'text/plain'
                    },
                    body: JSON.stringify({ email, password: newPassword })
                });

                if (!registerResponse.ok) {
                    throw new Error('Failed to register user');
                }

                const registerData = await registerResponse.json();
                userId = registerData.userId;

                if (!userId) {
                    throw new Error('User ID not found after registration');
                }

                // Send an email with the new password
                const passwordEmailResponse = await fetch('https://localhost:7207/Mail', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        emailToId: email,
                        emailToName: email,
                        emailSubject: `Welcome to Hotel None Inclusive`,
                        emailBody: `Dear customer,\n\nThank you for registering with us! Here is your password: ${newPassword}\n\nPlease keep it safe.\n\nBest regards,\nHotel None Inclusive`
                    })
                });

                if (passwordEmailResponse.ok) {
                    console.log('Password email sent successfully.');
                } else {
                    console.error('Failed to send password email.');
                }
            } else {
                throw new Error('Failed to check email');
            }

            // Adjust dates for timezone offset
            const adjustForTimezone = (date) => {
                const offset = date.getTimezoneOffset();
                return new Date(date.getTime() - (offset * 60 * 1000)).toISOString();
            };

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

            const response = await fetch('https://localhost:7207/api/Booking', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(booking)
            });

            if (response.ok) {
                setAvailabilityMessage('Booking successful.');

                // Send a confirmation email
                const emailResponse = await fetch('https://localhost:7207/Mail', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        emailToId: email,
                        emailToName: email,
                        emailSubject: `Booking Confirmation`,
                        emailBody: `Dear customer,\n\nThank you for booking with us! Here are your booking details:\n\nCheck-in Date: ${new Date(checkInDate).toLocaleDateString()}\nCheck-out Date: ${new Date(checkOutDate).toLocaleDateString()}\n\nWe look forward to your stay!\n\nBest regards,\nHotel None Inclusive`
                    })
                });

                if (emailResponse.ok) {
                    console.log('Confirmation email sent successfully.');
                } else {
                    console.error('Failed to send confirmation email.');
                }

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
                    {roomType} Værelse
                </Typography>
                <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                    {description}
                </Typography>
                <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                    Etage: {floor}
                </Typography>
                <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                    Værelsesnummer: {roomNumber}
                </Typography>
                <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                    Pris: {price} DKK pr. nat
                </Typography>
            </CardContent>
            <CardActions>
                <Button size="small" color="primary" onClick={handleClickOpen}>
                    Reserver
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
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', width: '100%', flexDirection: 'column', gap: 2 }}>
                        <TextField
                            label="Kunde Email"
                            variant="outlined"
                            fullWidth
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                        />
                        <Box sx={{ display: 'flex', justifyContent: 'space-between', width: '100%' }}>
                            <Button onClick={handleClose} color="secondary">Cancel</Button>
                            <Button onClick={handleConfirm} color="primary">Confirm</Button>
                        </Box>
                    </Box>
                    <Button onClick={checkRoomAvailability} color="primary">Check Room Availability</Button>
                    {availabilityMessage && <Typography variant="body2">{availabilityMessage}</Typography>}
                    {totalPrice !== null && <Typography variant="body2">Total Price: ${totalPrice}</Typography>}
                </Box>
            </Modal>
        </Card>
    );
}
