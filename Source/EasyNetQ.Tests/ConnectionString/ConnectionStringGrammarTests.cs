﻿// ReSharper disable InconsistentNaming

using System.Linq;
using EasyNetQ.ConnectionString;
using FluentAssertions;
using Xunit;
using Sprache;

namespace EasyNetQ.Tests.ConnectionString
{
    public class ConnectionStringGrammarTests
    {
        [Fact]
        public void Should_parse_host()
        {
            var host = ConnectionStringGrammar.Host.Parse("my.host.com:1234");

            host.Host.Should().Be("my.host.com");
            host.Port.Should().Be(1234);
        }

        [Fact]
        public void Should_parse_host_without_port()
        {
            var host = ConnectionStringGrammar.Host.Parse("my.host.com");

            host.Host.Should().Be("my.host.com");
            host.Port.Should().Be(0);
        }

        [Fact]
        public void Should_parse_list_of_hosts()
        {
            var hosts = ConnectionStringGrammar.Hosts.Parse("host.one:1001,host.two:1002,host.three:1003");

            hosts.Count().Should().Be(3);
            hosts.ElementAt(0).Host.Should().Be("host.one");
            hosts.ElementAt(0).Port.Should().Be((ushort)1001);
            hosts.ElementAt(1).Host.Should().Be("host.two");
            hosts.ElementAt(1).Port.Should().Be((ushort)1002);
            hosts.ElementAt(2).Host.Should().Be("host.three");
            hosts.ElementAt(2).Port.Should().Be((ushort)1003);
        }

        [Fact]
        public void Should_parse_amqp()
        {
            var hosts = ConnectionStringGrammar.AMQP.Parse("amqp://localhost/");

            hosts.Port.Should().Be(-1);
            hosts.Host.Should().Be("localhost");
        } 
        
        [Fact]
        public void Should_try_to_parse_amqp()
        {
            var message = "asd";
            var exception = Assert.Throws<ParseException>(() => ConnectionStringGrammar.AMQP.Parse(message));

            Assert.Contains(message, exception.Message);
        }

        [Fact]
        public void Should_throw_when_parsing_empty()
        {
            Assert.Throws<ParseException>(() => ConnectionStringGrammar.ConnectionStringBuilder.Parse(""));
        }
    }


}

// ReSharper restore InconsistentNaming