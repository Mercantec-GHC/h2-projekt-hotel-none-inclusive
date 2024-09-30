import { MdOutlinePool } from "react-icons/md";
import { IoRestaurant } from "react-icons/io5";
import { FaSpa } from "react-icons/fa";
import { MdOutlineFitnessCenter } from "react-icons/md";
import { FaWifi } from "react-icons/fa";
import { MdRoomService } from "react-icons/md";
import { BiSolidDrink } from "react-icons/bi";
import { MdLocalLaundryService } from "react-icons/md";
import { MdFreeBreakfast } from "react-icons/md";
import { LuAirVent } from "react-icons/lu";


const facilities = [
    {
        id: 1,
        facility: "Swimming Pool",
        icon: <MdOutlinePool color="black" />,
    },
    {
        id: 2,
        facility: "Restaurant",
        icon: <IoRestaurant color="black" />,
    },
    {
        id: 3,
        facility: "Spa & Wellness",
        icon: <FaSpa color="black" />,
    },
    {
        id: 4,
        facility: "Fitness Center",
        icon: <MdOutlineFitnessCenter color="black" />,
    },
    {
        id: 5,
        facility: "Gratis Wi-Fi",
        icon: <FaWifi color="black" />,
    },
    {
        id: 6,
        facility: "Room Service",
        icon: <MdRoomService color="black" />,
    },
    {
        id: 7,
        facility: "Bar/Lounge",
        icon: <BiSolidDrink color="black" />,
    },
    {
        id: 8,
        facility: "Laundry Service",
        icon: <MdLocalLaundryService color="black" />,
    },
    {
        id: 9,
        facility: "Breakfast Buffet",
        icon: <MdFreeBreakfast color="black" />,
    },
    {
        id: 10,
        facility: "Air Conditioning",
        icon: <LuAirVent color="black" />,
    },
];

export default facilities;