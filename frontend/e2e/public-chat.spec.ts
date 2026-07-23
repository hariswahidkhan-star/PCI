import { test, expect } from '@playwright/test'

// First-party site chat widget (assets/chat.js + Endpoints/Chat.cs): a bot greets on start and
// answers every message — small talk, knowledge base, or a deterministic fallback — so a reply
// is guaranteed for any input. Each fresh browser context starts a brand-new conversation
// (the token lives in localStorage), keeping runs independent.
test.describe('site chat widget', () => {
  test('a visitor can open the chat, send a message and receive a bot reply', async ({ page }) => {
    await page.goto('/')

    await page.getByRole('button', { name: 'Open chat with the PCI Assistant' }).click()

    // Starting the conversation renders the bot greeting as the first message.
    const botMessages = page.locator('.pci-chat-msg.bot')
    await expect(botMessages).toHaveCount(1)

    // Deliberately nonsensical and free of the handoff words (human/agent/person/someone/live),
    // so the bot answers itself instead of queueing for a team member.
    await page.getByLabel('Type your message').fill('zzqx quasar flibbertigibbet')
    await page.getByRole('button', { name: 'Send message' }).click()

    // The visitor message renders, then the bot reply arrives via the widget's poll. The widget
    // re-fetches the full history after sending (the greeting can render once more with its server
    // id), so assert "a second bot message exists" rather than an exact count.
    await expect(page.locator('.pci-chat-msg.visitor')).toHaveCount(1)
    await expect(botMessages.nth(1)).toBeVisible()
  })
})
