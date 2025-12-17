## Elevator pitch (1-2 sentances)
Wanderlust AI is a chat-first travel planner that turns a simple prompt into a personalized itinerary using real Yelp places, then helps you refine it and even book reservations.
It’s your AI travel companion for discovering, planning, and navigating a trip end-to-end.

## Inspiration
Trip planning usually happens across too many tabs: “where should we eat?”, “what’s nearby?”, “can we get a table?”, “how far is that from where we are?”.
We wanted to compress that whole workflow into a single conversation—powered by real business data—so you can plan a great trip without doing the busywork.

## What it does
- Lets you chat with a primary travel agent (Wanderlust AI) to describe your trip, preferences, dates, and constraints.
- Uses Yelp AI to discover real restaurants/attractions/services and summarize recommendations.
- Generates a structured, day-by-day travel itinerary and displays it in an itinerary dashboard.
- Saves itineraries in the browser and lets you reload a prior itinerary (optionally with its chat history).
- Shows driving directions on an embedded map from your current location to a selected itinerary stop.
- Supports Yelp bookings workflows (openings, holds, reservations, status, cancel) via tool calls.

## How we built it
- UI: Blazor Server (.NET 10) with interactive server components and SignalR streaming.
- Agent orchestration: `Microsoft.Agents.AI` + `Microsoft.Extensions.AI` with tool/function calling.
- LLM provider: OpenRouter via the OpenAI .NET client configured with the OpenRouter endpoint.
  - Primary agent defaults to `openai/gpt-5.1`.
  - Itinerary generation uses a separate agent with JSON-schema output (e.g., `openai/gpt-oss-120b`).
- Data + actions exposed to the agent as tools:
  - Yelp AI Chat API for discovery (`https://api.yelp.com/ai/chat/v2`).
  - Yelp Bookings endpoints for reservation workflows.
  - Weather via Open-Meteo.
  - Itinerary creation/modification tools.
- Location + maps:
  - Browser geolocation + reverse geocoding (Nominatim / OpenStreetMap) to turn coordinates into a human-readable starting point.
  - Google Maps JavaScript API for directions + markers, called via JS interop.
- Persistence:
  - Itineraries and serialized agent threads are stored in browser local storage so you can come back later.

## Challenges we ran into
- Streaming UI: keeping the chat responsive while tokens stream and avoiding excessive re-rendering.
- Tool outputs: Yelp AI returns JSON, but the UI needs friendly summaries and consistent formatting.
- Multi-turn context: persisting/rehydrating agent threads and associating them with saved itineraries.
- Maps integration: coordinating JS interop, DOM lifecycle, and map updates as destinations change.
- Secret management: keeping API keys out of source while still making local dev straightforward.

## Accomplishments that we're proud of
- A clean “Chat / Itinerary” workflow that feels like a single experience, not separate pages.
- Real place grounding: recommendations come from Yelp APIs, not hallucinated venues.
- One-click return trips: saved itineraries (and optional chat history) load back into the session.
- In-context navigation: map directions pop up directly from itinerary selections.

## What we learned
- Tool calling becomes dramatically more useful when the UI reflects tool outcomes (itinerary cards, maps, saved state).
- Persisting agent threads makes the experience feel continuous—but requires careful UX around “clear session” vs “load history”.
- Blazor Server + streaming is a strong fit for conversational UX when you throttle renders intentionally.

## What's next for Wanderlust AI
- Add a first-class “edit itinerary” flow (inline swaps, drag/drop, and smarter constraints).
- Improve booking UX (confirmations, error recovery, and clearer reservation summaries).
- Expand map modes (walking/transit, multi-stop routing) and deeper itinerary-to-map visualization.
- Harden configuration and secrets (documented setup, validation, and safer defaults).

## Built with
- .NET 10 + ASP.NET Core Blazor Server
- SignalR
- Microsoft.Agents.AI + Microsoft.Extensions.AI (tool calling)
- OpenAI .NET client configured for OpenRouter
- OpenRouter models (e.g., `openai/gpt-5.1`, `openai/gpt-oss-120b`)
- Yelp AI Chat API
- Yelp Bookings endpoints
- Google Maps JavaScript API
- OpenStreetMap Nominatim (reverse geocoding)
- Open-Meteo (.NET)
- Blazored.LocalStorage
- Markdig
