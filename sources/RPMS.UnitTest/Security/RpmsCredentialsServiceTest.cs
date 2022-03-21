using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RPMS.Common.Security;

namespace RPMS.UnitTest.Security
{
    [TestClass]
    public class RpmsCredentialsServiceTest
    {
        private readonly Mock<ICryptographyService> _cryptographyServiceMock = new Mock<ICryptographyService>();

        private readonly Mock<IRpmsCredentialsRepository> _rpmsCredentialsRepositoryMock =
            new Mock<IRpmsCredentialsRepository>();

        [TestInitialize]
        public void OnInitialize()
        {
            _rpmsCredentialsRepositoryMock.Setup(x => x.Get())
                .Returns(RpmsCredentialsMotherObject.CreateEncyptedCredentials);
        }

        protected RpmsCredentialsService CreateSut()
        {
            return new RpmsCredentialsService(_rpmsCredentialsRepositoryMock.Object, _cryptographyServiceMock.Object);
        }

        [TestCategory("Rpms")]
        [TestMethod]
        public void It_reads_encrypted_credentials_from_repository()
        {
            RpmsCredentialsService sut = CreateSut();

            sut.GetCredentials().Should().NotBeNull();

            _rpmsCredentialsRepositoryMock.Verify(x => x.Get(), Times.Once());
        }

        [TestCategory("Rpms")]
        [TestMethod]
        public void It_decrypt_when_read()
        {
            RpmsCredentialsService sut = CreateSut();

            sut.GetCredentials().Should().NotBeNull();

            _cryptographyServiceMock.Verify(x => x.Decrypt(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestCategory("Rpms")]
        [TestMethod]
        public void It_save_encrypted_credentials()
        {
            RpmsCredentials openTextCredentials = RpmsCredentialsMotherObject.CreateOpenTextCredentials();
            RpmsCredentials credentials = null;

            _rpmsCredentialsRepositoryMock.Setup(x => x.Add(It.IsAny<RpmsCredentials>()))
                .Callback<RpmsCredentials>(x => credentials = x);
            _cryptographyServiceMock.Setup(x => x.Encrypt(It.Is<string>(y => y == openTextCredentials.AccessCode)))
                .Returns("AccessCodeEncrypted");
            _cryptographyServiceMock.Setup(x => x.Encrypt(It.Is<string>(y => y == openTextCredentials.VerifyCode)))
                .Returns("VerifyCodeEncrypted");

            RpmsCredentialsService sut = CreateSut();

            sut.AddCredentials(openTextCredentials);

            _rpmsCredentialsRepositoryMock.Verify(x => x.Add(It.IsAny<RpmsCredentials>()), Times.Once());

            credentials.Should().NotBeNull();
            credentials.AccessCode.Should().NotBeEmpty();
            credentials.VerifyCode.Should().NotBeEmpty();

            credentials.AccessCode.Should().NotBe(openTextCredentials.AccessCode);
            credentials.VerifyCode.Should().NotBe(openTextCredentials.VerifyCode);
            credentials.ExpireAt.Should().Be(openTextCredentials.ExpireAt);
        }

        [TestCategory("Rpms")]
        [TestMethod]
        public void It_creates_new_id()
        {
            RpmsCredentials openTextCredentials = RpmsCredentialsMotherObject.CreateOpenTextCredentials();
            RpmsCredentials credentials = null;

            _rpmsCredentialsRepositoryMock.Setup(x => x.Add(It.IsAny<RpmsCredentials>()))
                .Callback<RpmsCredentials>(x => credentials = x);

            RpmsCredentialsService sut = CreateSut();

            sut.AddCredentials(openTextCredentials);

            _rpmsCredentialsRepositoryMock.Verify(x => x.Add(It.IsAny<RpmsCredentials>()), Times.Once());
            credentials.Should().NotBeNull();
            credentials.Id.Should().NotBeEmpty();
        }


        [TestCategory("Rpms")]
        [TestMethod]
        public void It_deletes_existing()
        {
            Guid id = Guid.NewGuid();
            RpmsCredentialsService sut = CreateSut();

            sut.DeleteCredentials(id);

            _rpmsCredentialsRepositoryMock.Verify(x => x.Delete(id), Times.Once());
        }
    }
}