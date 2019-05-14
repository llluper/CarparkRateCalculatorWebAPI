using System;
using carparkRateCalculator.Models;
using Xunit;

namespace carparkRateCalculatorTests
{
    public class TicketMachineTest
    {
        Ticket ticket = new Ticket();
        TicketMachine ticketMachine;
        DateTime entryEarly = DateTime.Parse("5/3/2019 8:30:52 AM");
        DateTime entryLate = DateTime.Parse("5/3/2019 8:30:52 PM");
        DateTime exitLate = DateTime.Parse("5/4/2019 6:30:52 PM");
        DateTime exitWeekend = DateTime.Parse("5/5/2019 6:30:52 PM");

        [Fact]
        public void CheckTicketType()
        {
            ticket.entry = entryEarly;
            ticket.exit = exitLate;
            ticketMachine = new TicketMachine(ticket);
            Assert.IsType<Ticket>(ticket);
        }

        [Fact]
        public void IsValidTest()
        {
            ticket.entry = entryEarly;
            ticket.exit = exitLate;
            ticketMachine = new TicketMachine(ticket);
            var okResult = ticketMachine.IsValid();
            Assert.True(okResult);
        }

        [Fact]
        public void IsValidFailTest()
        {
            ticket.entry = exitLate;
            ticket.exit = entryEarly;
            ticketMachine = new TicketMachine(ticket);
            var falseResult = ticketMachine.IsValid();
            Assert.False(falseResult);
        }

        [Fact]
        public void IsWeekendTest()
        {
            ticket.entry = entryEarly;
            ticket.exit = exitLate;
            ticketMachine = new TicketMachine(ticket);
            var trueResult = ticketMachine.IsWeekend(ticketMachine.exit.DayOfWeek);

            Assert.True(trueResult);
        }

        [Fact]
        public void IsWeekdayTest()
        {
            ticket.entry = entryEarly;
            ticket.exit = exitLate;
            ticketMachine = new TicketMachine(ticket);
            var trueResult = !ticketMachine.IsWeekend(ticketMachine.entry.DayOfWeek);

            Assert.True(trueResult);
        }

        [Fact]
        public void CheckEntryConditionEarlyTest()
        {
            ticket.entry = entryEarly;
            ticket.exit = exitLate;
            ticketMachine = new TicketMachine(ticket);
            ticketMachine.CheckEntryCondition();

            Assert.Equal(TicketMachine.RateType.Early, ticketMachine.entryType);
        }

        [Fact]
        public void CheckEntryConditionNightTest()
        {
            ticket.entry = entryLate;
            ticket.exit = exitLate;
            ticketMachine = new TicketMachine(ticket);
            ticketMachine.CheckEntryCondition();

            Assert.Equal(TicketMachine.RateType.Night, ticketMachine.entryType);
        }

        [Fact]
        public void CheckEntryConditionWeekendTest()
        {
            ticket.entry = exitLate;
            ticket.exit = exitLate;
            ticketMachine = new TicketMachine(ticket);
            ticketMachine.CheckEntryCondition();

            Assert.Equal(TicketMachine.RateType.Weekend, ticketMachine.entryType);
        }

        [Fact]
        public void CheckExitConditionEarlyTest()
        {
            ticket.entry = entryEarly;
            ticket.exit = entryLate;
            ticketMachine = new TicketMachine(ticket);
            ticketMachine.CheckEntryCondition();
            ticketMachine.CheckExitCondition();

            Assert.Equal(TicketMachine.RateType.Early, ticketMachine.entryType);
        }

        [Fact]
        public void CheckExitConditionNightTest()
        {
            ticket.entry = entryLate;
            ticket.exit = exitLate;
            ticketMachine = new TicketMachine(ticket);
            ticketMachine.CheckEntryCondition();
            ticketMachine.CheckExitCondition();

            Assert.Equal(TicketMachine.RateType.Night, ticketMachine.entryType);
        }

        [Fact]
        public void CheckExitConditionWeekendTest()
        {
            ticket.entry = exitLate;
            ticket.exit = exitWeekend;
            ticketMachine = new TicketMachine(ticket);
            ticketMachine.CheckEntryCondition();
            ticketMachine.CheckExitCondition();

            Assert.Equal(TicketMachine.RateType.Weekend, ticketMachine.entryType);
        }
    }
}
